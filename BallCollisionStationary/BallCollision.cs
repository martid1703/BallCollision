using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace BallCollisionStationary
{

    public partial class BallCollision : Form
    {
        Bitmap bmp;
        Graphics graph;
        int cursorRad;
        PointF cursor, cursorPrev, pObs, p0, p1,p2, p1Prev, t1, t2, pa, pb, paPrev, pbPrev;
        PointF ipaprev, p0prev, p2prev;
        float xObs, yObs, rObs, rp1;

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            graph.Clear(bgc);
            if (i == 0)
                i = 1;
            nValue += (1 / i);
            lbl_nValue.Text = "nValue= " +Math.Round(nValue,2);
            if (nValue >= 10)
            {
                nValue = 1;
                i = 5;
            }
            i -= 1;
        }

        float cx, cy, cxPrev, cyPrev, sizex, sizey;
        Color bgc;
        Pen drawPen;
        Pen erasePen;
        Brush drawBrush = Brushes.Yellow;
        Brush eraseBrush = Brushes.DarkBlue;
        Circle cObs, ic, cp1,cp1pa;
        Line pObsp1, lineT;
        double i,nValue=1;


        public BallCollision()
        {
            InitializeComponent();
            Init();
            //timerNValue.Enabled = true;
            i = 5;
        }
        public void Init()
        {
            bgc = Color.DarkBlue;
            drawPen = new Pen(Color.Yellow);
            erasePen = new Pen(bgc);

            bmp = new Bitmap(pictureBox.Width, pictureBox.Height, PixelFormat.Format24bppRgb);
            graph = Graphics.FromImage(bmp);
            //заполняем фоновым цветом фон
            graph.FillRectangle(eraseBrush, 0, 0, bmp.Width, bmp.Height);

            rObs = pictureBox.Width / 6;
            xObs = pictureBox.Width / 2;
            yObs = pictureBox.Height / 2;
            pObs = new Point((int)xObs, (int)yObs);
            rp1 = rObs / 5;
            cursorRad = 5;

            cObs = new Circle(pObs, rObs);
            graph.DrawEllipse(drawPen,
            xObs - rObs,
            yObs - rObs,
            rObs * 2,
            rObs * 2);
            graph.FillRectangle(Brushes.Red, pObs.X, pObs.Y, 2, 2);
            //--отрисовываем форму----
            pictureBox.Image = bmp;
        }
        private void HighlightPoint(PointF p)
        {
            graph.FillEllipse(drawBrush, p.X - cursorRad, p.Y - cursorRad, cursorRad*2, cursorRad*2);
        }
        private void DeHighlightPoint(PointF p)
        {
            graph.FillEllipse(eraseBrush, p.X - cursorRad, p.Y - cursorRad, cursorRad * 2, cursorRad * 2);
        }
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            #region РАЗДЕЛ № 1: 
            //-----------------------------------------------
            //------РАЗДЕЛ № 1:          --------------------
            //-----------------------------------------------

            //---окружность, на которой должна находится т.р1-----
            ic = new Circle(pObs, rObs + rp1);
            //----------------------------------
            graph.DrawEllipse(drawPen,
            xObs - rObs,
            yObs - rObs,
            rObs * 2,
            rObs * 2);

            double lengthLineT = ic.radius;//некая константа, может быть любой

            //обработка положения курсора
            cursorPrev = cursor;
            graph.FillEllipse(eraseBrush, cursorPrev.X - cursorRad, cursorPrev.Y - cursorRad, cursorRad * 2, cursorRad * 2);
            cursor = this.PointToClient(Cursor.Position);
            graph.FillEllipse(Brushes.Red, cursor.X - cursorRad, cursor.Y - cursorRad, cursorRad * 2, cursorRad * 2);

            //-----находим пересечение окружности ic и прямой, на которой должна лежать т.р1---------
            int ip;
            ip = Calculations.FindLineCircleIntersections(
                ic.center.X,
                ic.center.Y,
                (float)ic.radius,
                pObs,
                cursor,
                out t1, out t2);

            p1Prev = p1;
            graph.DrawLine(erasePen, pObs, p1);

            //--какая т.t1 или т.t2 ближе к положению курсора, ту и оставляем и записываем в текущее положение т.p1
            p1.X = t1.X;
            p1.Y = t1.Y;
            //----отрисовываем соединительную линию между pObs, p1.
            graph.DrawLine(drawPen, pObs, p1);
            //------составляем уравнение тангенсоиды, проходящий через p1----------
            if (pObs.X == p1.X)
            {
                pa = new PointF(p1.X + (float)lengthLineT / 2, p1.Y);
                pb = new PointF(p1.X - (float)lengthLineT / 2, p1.Y);
            }
            else
            {
                if (pObs.Y == p1.Y)
                {
                    pa = new PointF(p1.X, p1.Y + (float)lengthLineT / 2);
                    pb = new PointF(p1.X, p1.Y - (float)lengthLineT / 2);
                }
                else
                {
                    pObsp1 = new Line(pObs, p1);
                    double k, b;
                    k = -1 / pObsp1.k;
                    b = p1.Y - k * p1.X;
                    lineT = new Line(k, b);
                    //---Вычисляем отрезок ра-рb на прямой тангенсоиды, на основании точки р1 и его длины------
                    lineT.CalculateLineSegment(p1, (int)lengthLineT, out pa, out pb);
                }
            }
            PointF pt;
            if (p1.Y < pObs.Y)
            {
                pt = pa;
                pa = pb;
                pb = pt;
            }
            //---Стираем отрезок ра-рb, который есть тангенсоида в т.р1--------
            graph.DrawLine(erasePen, paPrev, pbPrev);
            DeHighlightPoint(paPrev);
            //--для целей стирания нужны предыдущие точки отрезка----
            paPrev = pa;
            pbPrev = pb;
            //---Отрисовываем отрезок ра-рb, который есть тангенсоида в т.р1--------
            graph.DrawLine(drawPen, pa, pb);
            HighlightPoint(pa);
            //-----------------------------------------

            //---Уравнение окружности шара в т. р1--------
            cp1 = new Circle(p1, rp1);
            //-----------------------
            cxPrev = cx;
            cyPrev = cy;
            cx = (float)(p1.X - cp1.radius);
            cy = (float)(p1.Y - cp1.radius);
            sizex = (float)(cp1.radius * 2);
            sizey = (float)(cp1.radius * 2);
            //---Стираем шар в т. р1--------
            graph.DrawEllipse(erasePen, cxPrev, cyPrev, sizex, sizey);
            graph.DrawEllipse(erasePen, p1Prev.X- (float)lengthLineT/2, p1Prev.Y- (float)lengthLineT / 2, (float)lengthLineT / 2 * 2, (float)lengthLineT / 2 * 2);

            //---Рисуем шар в т. р1--------
            graph.DrawEllipse(drawPen, cx, cy, sizex, sizey);
            graph.DrawEllipse(drawPen, p1.X - (float)lengthLineT / 2, p1.Y - (float)lengthLineT / 2, (float)lengthLineT / 2 * 2, (float)lengthLineT / 2 * 2);

            #endregion
            #region РАЗДЕЛ № 2: Вычисляем положение р0
            //-----------------------------------------------
            //----РАЗДЕЛ № 2: Вычисляем положение р0 --------
            //-----------------------------------------------
            //---Уравнение окружности 'cp1pa', на которой должны находится р0 и р2-----------
            cp1pa = new Circle(p1, lengthLineT / 2);
            //-------Вычисляем окружность 'cObsm', касающуюся тангенсоиды в случайном месте между р0 и р1-----------
            double dpObspa = Calculations.DistanceBetweenTwoPoints(pObs, pa);
            double dpObsp1 = Calculations.DistanceBetweenTwoPoints(pObs, p1);
            
            
            
            //-----------------------------------------------------------.
            double delta = (dpObspa - dpObsp1) / nValue;



            //-----------------------------------------
            Circle cObsm = new Circle(pObs, dpObsp1 + delta);
            //---Стираем шар в т. р1--------
            //cObsm.Draw(graph, erasePen);
            //---Рисуем шар в т. р1--------
            cObsm.Draw(graph, drawPen);
            //-находим пересечение окружности cObsm и тангенсоиды---
            PointF ipa;
            int ans = Calculations.FindLineCircleIntersections(cObsm.center.X, cObsm.center.Y, (float)cObsm.radius, pa, pb, out t1, out t2);
            //-Какая из точек ближе к pa, ту и оставляем-

            double dipap0 = Calculations.DistanceBetweenTwoPoints(t1, pa);
            double dipbp0 = Calculations.DistanceBetweenTwoPoints(t2, pa);
            if (dipap0 < dipbp0)
                ipa = t1;
            else
                ipa = t2;
            //--находим пересечение прямой lineIP в т.ipa и окружности cp1p0---
            PointF icipa, icipb;
            if (pObs.X == p1.X)
            {
                PointF ipa2 = new PointF(ipa.X, ipa.Y + 1);//+1 - тут может быть любое число, линия вертикальная
                int z = Calculations.FindLineCircleIntersections(p1.X, p1.Y, (float)cp1.radius, ipa, ipa2, out icipa, out icipb);
            }
            else
            {
                if (pObs.Y == p1.Y)
                {
                    PointF ipa2 = new PointF(ipa.X + 1, ipa.Y);//+1 - тут может быть любое число, линия горизонтальная
                    int z = Calculations.FindLineCircleIntersections(p1.X, p1.Y, (float)cp1.radius, ipa, ipa2, out icipa, out icipb);
                }
                else
                {

                    //-уравнение прямой, проходящей через точку пересечения 'ipa' и параллельной прямой pObsp1---
                    double kip, bip;
                    kip = pObsp1.k;
                    bip = ipa.Y - kip * ipa.X;
                    Line lineIP = new Line(kip, bip);
                    PointF ipa2 = new PointF((float)(-bip / kip), 0);
                    int z = Calculations.FindLineCircleIntersections(p1.X,p1.Y,(float)cp1pa.radius,ipa,ipa2,out icipa, out icipb);
                }
            }

            //-------Какая из точек дальше от pObs, ту и оставляем-------

            graph.DrawEllipse(erasePen, p0.X - rp1, p0.Y - rp1, rp1 * 2, rp1 * 2);
       
            graph.DrawLine(erasePen, ipaprev, p0prev);

            double dicipapObs = Calculations.DistanceBetweenTwoPoints(icipa, pObs);
            double dicipbpObs = Calculations.DistanceBetweenTwoPoints(icipb, pObs);
            if (dicipapObs > dicipbpObs)
                p0=icipa;
            else
                p0=icipb;
            ipaprev = ipa;
            p0prev = p0;
            graph.DrawLine(drawPen, ipa, p0);

            //-------рисуем окружность радиусом rp1 в т.p0---------
            graph.DrawEllipse(drawPen, p0.X - rp1, p0.Y - rp1, rp1 * 2, rp1 * 2);

            #endregion
            #region РАЗДЕЛ № 3: Вычисляем положение т.р2
            //-----------------------------------------------
            //----РАЗДЕЛ № 3: Вычисляем положение т.р2 --------
            //-----------------------------------------------
            //--находим пересечение прямой lineP2 в т.p0 и окружности cp1p0---
            PointF p2a, p2b;
            //p2t - временная вспомогательная точка для построения прямой lineP2
            if (pObs.X == p1.X)//если pObsp1 - вертикальная, то lineP2 будет горизонтальная
            {
                PointF p2t = new PointF(p0.X, p0.Y + 1);//+1 - тут может быть любое число, линия вертикальная
                int z = Calculations.FindLineCircleIntersections(p1.X, p1.Y, (float)cp1.radius, p0, p2t, out p2a, out p2b);
            }
            else
            {
                if (pObs.Y == p1.Y)//если pObsp1 - горизонтальная, то lineP2 будет вертикальная
                {
                    PointF p2t = new PointF(p0.X + 1, p0.Y);//+1 - тут может быть любое число, линия горизонтальная
                    int z = Calculations.FindLineCircleIntersections(p1.X, p1.Y, (float)cp1.radius, p0, p2t, out p2a, out p2b);
                }
                else
                {

                    //-уравнение прямой, проходящей через точку пересечения 'p0' и параллельной прямой lineT---
                    double kip, bip;
                    kip = lineT.k;
                    bip = p0.Y - kip * p0.X;
                    Line lineP2 = new Line(kip, bip);
                    PointF p2t = new PointF((float)(-bip / kip), 0);//находим вспомогательную точку, когда у=0
                    int z = Calculations.FindLineCircleIntersections(p1.X, p1.Y, (float)cp1pa.radius, p0, p2t, out p2a, out p2b);
                }
            }
            //-------Какая из точек дальше от p0, ту и оставляем-------

            graph.DrawEllipse(erasePen, p2.X - rp1, p2.Y - rp1, rp1 * 2, rp1 * 2);

            graph.DrawLine(erasePen, ipaprev, p0prev);

            double dp2a = Calculations.DistanceBetweenTwoPoints(p0, p2a);
            double dp2b = Calculations.DistanceBetweenTwoPoints(p0, p2b);
            if (dp2a > dp2b)
                p2 = p2a;
            else
                p2 = p2b;

            p2prev = p2;
            p0prev = p0;
            graph.DrawLine(drawPen, p2, p0);

            //-------рисуем окружность радиусом rp1 в т.p2---------
            graph.DrawEllipse(drawPen, p2.X - rp1, p2.Y - rp1, rp1 * 2, rp1 * 2);

            //-----------------------------
            #endregion
            pictureBox.Image = bmp;
            }
        }
    }
