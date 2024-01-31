using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Core.Laser
{
    /// <summary>
    /// 3*3 행렬 스택 래퍼
    /// 조작이 있을때만 곱셉 연산을 하도록 효율 개선
    /// hcchoi
    /// 2020.9
    /// </summary>
    public class MatrixStack
    {
        /// <summary>
        /// 스택에 있는 3x3 행렬 개수
        /// </summary>
        public int Count
        {
            get { return this.stack.Count; }
        }

        /// <summary>
        /// 스택에 있는 3x3 행렬의 연산 결과
        /// </summary>
        public Matrix3x2 Result
        {
            get
            {
                if (!this.isModified)
                    return this.resultMatrix;
                this.resultMatrix = Matrix3x2.Identity;                
                foreach (var m in this.stack)
                    resultMatrix *= m;
                this.isModified = false;
                return this.resultMatrix;
            }
        }

        Stack<Matrix3x2> stack = new Stack<Matrix3x2>();
        Matrix3x2 resultMatrix;
        bool isModified;

        /// <summary>
        /// 생성자
        /// </summary>
        public MatrixStack()
        {
            this.stack.Push(Matrix3x2.Identity);
            this.isModified = true;
        }

        /// <summary>
        /// 스택에 행렬 넣기
        /// </summary>
        /// <param name="m"></param>
        public void Push(Matrix3x2 m)
        {
            this.stack.Push(m);
            this.isModified = true;
        }
        /// <summary>
        /// 스택에서 행렬 꺼내기
        /// </summary>
        /// <param name="m"></param>
        public void Pop(out Matrix3x2 m)
        {
            Debug.Assert(this.stack.Count > 1);
            m = this.stack.Pop();
            this.isModified = true;
        }
        /// <summary>
        /// 스택의 모든 행렬 지우고 단위행렬로 초기화
        /// </summary>
        public void Clear()
        {
            this.stack.Clear();
            this.stack.Push(Matrix3x2.Identity);
            this.isModified = true;
        }
        /// <summary>
        /// 입력된 벡터를 행렬스택과 곱한 결과 벡터 얻기
        /// </summary>
        /// <param name="vectorIn"></param>
        /// <param name="vectorOut"></param>
        public void CalculateVector(Vector2 vectorIn, out Vector2 vectorOut)
        {
            vectorOut = Vector2.Zero;
            vectorOut = Vector2.Transform(vectorIn, this.Result);
        }
        /// <summary>
        /// 입력된 벡터를 행렬스택과 곱한 결과 벡터 얻기
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="yIn"></param>
        /// <param name="xOut"></param>
        /// <param name="yOut"></param>
        /// <returns></returns>
        public void CalculateVector(float xIn, float yIn, out float xOut, out float yOut)
        {
            xOut = yOut = 0;
            var vectorResult = Vector2.Transform(new Vector2(xIn, yIn), this.Result);
            xOut = vectorResult.X;
            yOut = vectorResult.Y;
        }
    }
}
