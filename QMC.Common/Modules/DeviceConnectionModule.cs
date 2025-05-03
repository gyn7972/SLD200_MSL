using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.Modules
{
    public class DeviceConnectionModule
    {
        private System.Timers.Timer _checkTimer;
        private bool _isConnected = false;
        private bool _isChecking = false;

        public bool IsConnected => _isConnected;

        // 이벤트: 외부에 연결 상태 변화 통지
        public event Action<bool> OnStatusChanged;
        public event Action<string> OnErrorOccurred;
    

        public DeviceConnectionModule()
        {
            _checkTimer = new System.Timers.Timer(1000); // 1초 간격
            _checkTimer.Elapsed += async (s, e) => await CheckConnectionAsync();
            _checkTimer.AutoReset = true;
        }

        public void Start() => _checkTimer.Start();
        public void Stop() => _checkTimer.Stop();

        public async Task CheckConnectionAsync()
        {
            if (_isChecking)
                return;

            _isChecking = true;

            try
            {
                bool result = await Task.Run(() => TryConnectToDevice());
                if (_isConnected != result)
                {
                    _isConnected = result;
                    OnStatusChanged?.Invoke(_isConnected);
                }
            }
            catch (Exception ex)
            {
                OnErrorOccurred?.Invoke($"디바이스 연결 오류: {ex.Message}");
            }
            finally
            {
                _isChecking = false;
            }
        }

        private bool TryConnectToDevice()
        {
            // TODO: 실제 연결 확인 로직 구현
            // 예: SerialPort.IsOpen, TCP Socket.Ping, 장비 API 등
            Thread.Sleep(100); // 시뮬레이션 지연
            return new Random().Next(0, 2) == 1;
        }


    }
}
