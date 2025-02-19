using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLD200_MSL
{
    public class FormBaseConfiguration
    {
        public Color BaseBackColor = Color.FromArgb( 180, 180, 180);
        public Color PanelBackColor = Color.FromArgb(220, 220, 220);

        public Size ButtonSize { protected set; get; }
        public Size RecipeButtonSize { protected set; get; }
        public Size MainSize { protected set; get; }
        public Size TopSize { protected set; get; }
        public Size BottomSize { protected set; get; }
        public Size ContentSize { protected set; get; }
        public Size BottomButtonSize { protected set; get; }
        public Size LockButtonSize { protected set; get; }
        public Size TopButtonSize { protected set; get; }
        public Size TopButtonLogoSize { protected set; get; }
        public Size TopButtonTimeSize { protected set; get; }
        public Size PanelSize { protected set; get; }        
        public Size ListBoxSize { protected set; get; }
        public Point ContentLocation { protected set; get; }

        public Size FormWithbuttonSize { protected set; get; }

        public Size FormContentSize { protected set; get; }
       
        public Size ControlsLocation { protected set; get; }

        public Size PanelbuttonSize { protected set; get; }

        public Size FunctionControlLocation { protected set; get; }

        public Size FunctionControlSize { protected set; get; }

        public Size JogControlLocation { protected set; get; }

        public Size JogControlSize { protected set; get; }
        public Size ModuleStateControlLocation { protected set; get; }

        public Size ModuleStateControlSize { protected set; get; }

        public Size VisionImageViewerSize { protected set; get; }
        

        public Size AlarmdataGridViewSize { protected set; get; }

        public Size AlarmGroupBoxSize { protected set; get; }

        public Size AlarmGroupBoxLocation { protected set; get; }
        public Size AlarmdataGridViewLocation { protected set; get; }

        public Size RecipeTreeViewSize { protected set; get; }
        public Size RecipeTreeViewLocation { protected set; get; }

        public Size RecipeSelectedStateSize { protected set; get; }
        public Size RecipeSelectedStateLocation { protected set; get; }
        public Size RecipeSetStateSize { protected set; get; }
        public Size RecipeSetStateLocation { protected set; get; }

        public Size MaintImageViewSize { protected set; get; }
        public Size ConfigMainPropertyGrdiSize { protected set; get; }
        public int ControlGap { protected set; get; }
        public int ConveyorControlGap { protected set; get; }

        public Size MaintMinImageViewSize { protected set; get; }
        public Size MonitorMainVisionImageViewerSize { protected set; get; }
        public Size MonitorSubVisionImageViewerSize { protected set; get; }

        public Size RecipePropertySize { protected set; get; }
        public Size MotorStatusControlFlowPanelSize { protected set; get; }
        public int ControlGap_2 { protected set; get; }
        public int ControlGap_3 { protected set; get; }

        public Size AxisConfigGridSize { protected set; get; }
        public Size AxisConfigPropertySize { protected set; get; }

        public Size IOGridSize { protected set; get; }
        public FormBaseConfiguration()
        {
        	//ButtonSize = new Size(100, 30);
            ButtonSize = new Size(150, 30);

            RecipeButtonSize = new Size(150, 40);

            MainSize = new Size(1920, 1070);

            TopSize = new Size(MainSize.Width, 95);

            BottomSize = new Size(MainSize.Width, 110);                                                         //  Bottom 메뉴 버튼 크기가 작아서, 패널 크기를 세로로 20 크게 변경. (90 -> 110)`

            ContentSize = new Size(MainSize.Width- 0,MainSize.Height-TopSize.Height-BottomSize.Height);

            //PanelSize = new Size(ContentSize.Width, ButtonSize.Height+3);
            PanelSize = new Size(ContentSize.Width, ButtonSize.Height + 12);

            BottomButtonSize = new Size(100, BottomSize.Height);
            LockButtonSize = new Size(70, 100);

            TopButtonSize = new Size(100, TopSize.Height);

            //TopButtonLogoSize = new Size(250, TopSize.Height);
            //TopButtonLogoSize = new Size(200, TopSize.Height - 30);
            TopButtonLogoSize = new Size(140, 30);

            TopButtonTimeSize = new Size(130, TopSize.Height);

            ListBoxSize = new Size((ContentSize.Width - ButtonSize.Width*2) / 4, ((ContentSize.Height - PanelSize.Height) /5) *2);


			ContentLocation = new Point(20, 38);
            //ContentLocation = new Point(10, 0);

            PanelbuttonSize = new Size(PanelSize.Width - ContentLocation.X, PanelSize.Height);

            FormWithbuttonSize = new Size(ContentSize.Width, ContentSize.Height - PanelSize.Height );

            FormContentSize = new Size(ContentSize.Width, FormWithbuttonSize.Height - PanelSize.Height);

            ControlsLocation = new Size(20, ButtonSize.Height);

            JogControlSize = new Size(600, 530);

            ModuleStateControlSize = new Size(130,210);

            FunctionControlSize = new Size(650,300);

            //VisionImageViewerSize = new Size(650, 544); // 370);              //  2448 x 2048 비율
            VisionImageViewerSize = new Size(360, 301);     //   (442, 370);     //   650, 544);     //  2448 x 2048 비율

            //Vision Module General 
            ModuleStateControlLocation = new Size(ContentLocation.X, PanelbuttonSize.Height);
            
            FunctionControlLocation = new Size(10, FormContentSize.Height-FunctionControlSize.Height- PanelSize.Height + PanelbuttonSize.Height);

            JogControlLocation = new Size(PanelSize.Width-JogControlSize.Width, 33);

            ConveyorControlGap = 33;

            AlarmdataGridViewSize = new Size(MainSize.Width -100, 400);

            AlarmGroupBoxSize = new Size(MainSize.Width - 100,320 );

            AlarmdataGridViewLocation = new Size(ContentLocation.X , 325);

            AlarmGroupBoxLocation = new Size(ContentLocation.X , 0);


            RecipeTreeViewSize = new Size(500, 700);
            RecipeTreeViewLocation = new Size(10, 200);

            RecipeSelectedStateSize = new Size(500, 700);
            RecipeSelectedStateLocation = new Size(700,200);

            RecipeSetStateSize = new Size(500, 600);
            RecipeSetStateLocation = new Size(3, 3);

            MaintImageViewSize = new Size(490, 280);

            ControlGap = 10;
            ConfigMainPropertyGrdiSize = new Size(335, 549);

            RecipePropertySize = new Size(250,540);
            //MaintMinImageViewSize = new Size(450, 400);
            MaintMinImageViewSize = new Size(450, 376);                     //  0.8366 비율로 바꿔봄
            MonitorMainVisionImageViewerSize = new Size(800,580);
            MonitorSubVisionImageViewerSize = new Size(210, 150);
            MotorStatusControlFlowPanelSize = new Size(175, 500);   // (190, 500);
            ControlGap_2 = 20;
            ControlGap_3 = 30;
            AxisConfigGridSize = new Size(800, 639);
            AxisConfigPropertySize = new Size(400, 639);

            IOGridSize = new Size(625, 800);
    }

    }
}
