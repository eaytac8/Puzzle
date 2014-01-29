using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace puzzle2
{
    public partial class Form1 : Form
    {
        Form frm;
        public static Graphics g;
        public static Image[,] parcalar;
        Image anaResim = Image.FromFile("resim.jpg");
        Tas[,] t;
        public static int zorluk = 3;
        bool oyunBasi = true;
        string kaynak = "resim.jpg";
        public int hamleSayisi = 0;
        public bool buyuyebilirMi = true;
        Panel ipucu = new Panel();

        public Form1()
        {
            InitializeComponent();
            if (oyunBasi)
            {
                toolStripStatusLabel1.Text = "Hamle Sayısı " + hamleSayisi.ToString();
                dilimle(zorluk);
                panel1.Visible = true;
                panel1.Refresh();
                oyunBasi = false;
            }
            this.Controls.Add(ipucu);
        }
        
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            g = panel1.CreateGraphics();                       
            ciz();
        }

        private void ciz()
        {
            foreach (Tas tass in t)
            {
                tass.Ciz();
            } 
        }

        private void randomla()
        {
            Random rnd = new Random();
            Tas tmp = new Tas(0,0,null,0);
            Tas t1 = new Tas(0, 0, null,0);
            Tas t2 = new Tas(0, 0, null,0);

            int tmpX = rnd.Next(zorluk);
            int tmpY = rnd.Next(zorluk);

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    t1 = t[i % zorluk, j % zorluk];
                    t2 = t[tmpX, tmpY];

                    tmp.posX = t1.posX;
                    tmp.posY = t1.posY;
                    t1.posX = t2.posX;
                    t1.posY = t2.posY;
                    t2.posX = tmp.posX;
                    t2.posY = tmp.posY;
                    
                    if (tmp.bosMu)
                    {
                        t1.bosMu = false;
                        t2.bosMu = true;
                    }

                    t[t1.posX, t1.posY] = t1;
                    t[t2.posX, t2.posY] = t2;                     

                    tmpX = rnd.Next(zorluk);
                    tmpY = rnd.Next(zorluk);
                }                
            }
        }
        
        private void dilimle(int zorluk)
        {
            int tasSayisi = 0;
            hamleSayisi = 0;
            toolStripStatusLabel1.Text = "Hamle Sayısı " + hamleSayisi.ToString();

            anaResim = Image.FromFile(kaynak);
            anaResim = resizeImage(anaResim, new Size(zorluk * 100, zorluk * 100));
            t = new Tas[zorluk, zorluk];
            parcalar = new Image[zorluk, zorluk];
            int boyut = 100;
            for (int i = 0; i < zorluk; i++)
            {
                for (int j = 0; j < zorluk; j++)
                {
                    tasSayisi++;
                    parcalar[i, j] = cropImage(anaResim, new Rectangle(i * boyut, j * boyut, 100, 100));
                    t[i,j] = new Tas(i,j,parcalar[i,j],tasSayisi);
                }
            }
            t[zorluk - 1, zorluk - 1] = new Tas(zorluk - 1, zorluk-1, Image.FromFile("bos.png"),tasSayisi);
            t[zorluk - 1, zorluk - 1].bosMu = true;

            randomla();
        }

        private static Image resizeImage(Image imgToResize, Size size)
        {
            Bitmap b = new Bitmap(zorluk * 100, zorluk * 100);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, zorluk * 100, zorluk * 100);
            g.Dispose();

            return (Image)b;
        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }
        
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X / 110 != zorluk - 1 && t[e.X / 110 + 1, e.Y / 110].bosMu) 
            {
                swap(t[e.X / 110, e.Y / 110], t[e.X / 110 + 1, e.Y / 110]);
            }

            else if (e.X / 110 != 0 && t[e.X / 110 - 1, e.Y / 110].bosMu)
            {
                swap(t[e.X / 110, e.Y / 110], t[e.X / 110 - 1, e.Y / 110]);
            }

            else if (e.Y / 110 != zorluk - 1 && t[e.X / 110, e.Y / 110 + 1].bosMu)
            {
                swap(t[e.X / 110, e.Y / 110], t[e.X / 110 , e.Y / 110 + 1]);
            }

            else if (e.Y / 110 != 0 && t[e.X / 110, e.Y / 110 - 1].bosMu)
            {
                swap(t[e.X / 110, e.Y / 110], t[e.X / 110, e.Y / 110 - 1]);
            }

            ciz();

            if (oyunBittiMi())
            {
                frm = new Form();
                frm.SetBounds(100, 100, 200, 220);
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                frm.Text = "Oyun Bitti";
                frm.TopMost = true;
                this.Enabled = false;
                frm.Load += new EventHandler(frm_Load);
                frm.Show();
                frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
            }
            
        }

        private void swap(Tas t1, Tas t2)
        {
            Tas t3 = new Tas(0,0,null,0);
            t3.posX = t1.posX;
            t3.posY = t1.posY;
            t1.posX = t2.posX;
            t1.posY = t2.posY;
            t2.posX = t3.posX;
            t2.posY = t3.posY;
            t3 = t[t1.posX, t1.posY];
            t[t1.posX, t1.posY] = t[t2.posX, t2.posY];
            t[t2.posX, t2.posY] = t3;

            hamleSayisi++;
            toolStripStatusLabel1.Text = "Hamle Sayısı " + hamleSayisi.ToString();
        }

        private bool oyunBittiMi()
        {
            int sayac = 1;
            
                for (int i = 0; i < zorluk; i++)
                {
                    for (int j = 0; j < zorluk; j++)
                    {
                        if (t[i, j].index != sayac)
                        {
                            return false;
                        } 
                        sayac++;
                    }
                }               
            
            return true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                kaynak = openFileDialog1.FileName;
                anaResim = Image.FromFile(kaynak);
                dilimle(zorluk);
                panel1.Refresh();
            }
            
        }

        private void kolayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zorluk = 3;
            dilimle(zorluk);            
            panel1.Size = new System.Drawing.Size(zorluk * 100 + ((zorluk - 1) * 10), zorluk * 100 + ((zorluk - 1) * 10));
            panel1.Visible = true;
            this.Size = new System.Drawing.Size(400,420);
            panel1.Refresh();            
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zorluk = 4;
            dilimle(zorluk);
            panel1.Size = new System.Drawing.Size(zorluk * 100 + ((zorluk - 1) * 10), zorluk * 100 + ((zorluk - 1) * 10));
            panel1.Visible = true;
            this.Size = new System.Drawing.Size(470,530);
            panel1.Refresh();            
        }

        private void zorToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            zorluk = 5;
            dilimle(zorluk);
            panel1.Size = new System.Drawing.Size(zorluk * 100 + ((zorluk - 1) * 10), zorluk * 100 + ((zorluk - 1) * 10));
            panel1.Visible = true;
            this.Size = new System.Drawing.Size(580,640);
            panel1.Refresh();            
        }

        private void extremeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zorluk = 6;
            dilimle(zorluk);
            panel1.Size = new System.Drawing.Size(zorluk * 100 + ((zorluk - 1) * 10), zorluk * 100 + ((zorluk - 1) * 10));
            panel1.Visible = true;
            this.Size = new System.Drawing.Size(690, 750);
            panel1.Refresh(); 
        }
        
        private void toolStripButton2_Click(object sender, EventArgs e)
        {           

            if (buyuyebilirMi)
            {
                toolStripDropDownButton1.Enabled = false;
                toolStripButton1.Enabled = false;
                toolStripButton3.Enabled = false;
                this.Size += new System.Drawing.Size(zorluk * 100 + ((zorluk - 1) * 10), 0);
                buyuyebilirMi = false;                
                ipucu.Size = new System.Drawing.Size(zorluk * 100, zorluk * 100 );
                ipucu.Location = new Point(panel1.Location.X + 20 + zorluk * 100 + ((zorluk - 1) * 10), panel1.Location.Y );
                
                ipucu.Paint += new PaintEventHandler(ipucu_Paint);
                ipucu.Visible = true;
                
                ipucu.Refresh();
                panel1.Refresh();       
            }

            else if (!buyuyebilirMi)
            {
                toolStripDropDownButton1.Enabled = true;
                toolStripButton1.Enabled = true;
                toolStripButton3.Enabled = true;
                this.Size -= new System.Drawing.Size(zorluk * 100 + ((zorluk - 1) * 10), 0);
                buyuyebilirMi = true;
                ipucu.Visible = false;
            }
            
        }       

        void frm_Load(object sender, EventArgs e)
        {
            Label lbl_bitti = new Label();
            lbl_bitti.SetBounds(0,10,190,30);
            lbl_bitti.Text = "Oyun Bitti!\nYeni Oyun İçin Zorluk Seçiniz";
            lbl_bitti.TextAlign = ContentAlignment.MiddleCenter;

            Button btn_kolay = new Button();
            btn_kolay.SetBounds(5, 50, 40, 40);
            btn_kolay.BackgroundImage = Image.FromFile("kolay.png");
            btn_kolay.BackgroundImageLayout = ImageLayout.Stretch;
            
            Button btn_normal = new Button();
            btn_normal.SetBounds(50, 50, 40, 40);
            btn_normal.BackgroundImage = Image.FromFile("normal.png");
            btn_normal.BackgroundImageLayout = ImageLayout.Stretch;

            Button btn_zor = new Button();
            btn_zor.SetBounds(95, 50, 40, 40);
            btn_zor.BackgroundImage = Image.FromFile("zor.png");
            btn_zor.BackgroundImageLayout = ImageLayout.Stretch;

            Button btn_extreme = new Button();
            btn_extreme.SetBounds(140, 50, 40, 40);
            btn_extreme.BackgroundImage = Image.FromFile("extreme.png");
            btn_extreme.BackgroundImageLayout = ImageLayout.Stretch;

            Label lbl_seviye = new Label();
            lbl_seviye.SetBounds(5, 90, 190, 12);
            lbl_seviye.Text = "  Kolay      Normal      Zor     Extreme";
            lbl_seviye.TextAlign = ContentAlignment.TopLeft;

            Panel pnl_bitisResmi = new Panel();
            pnl_bitisResmi.SetBounds(50,113,80,80);
            pnl_bitisResmi.BackgroundImage = Image.FromFile("gameover.png");
            pnl_bitisResmi.BackgroundImageLayout = ImageLayout.Stretch;

            btn_kolay.Click += new EventHandler(kolayToolStripMenuItem_Click);
            btn_kolay.Click += new EventHandler(formKapatma);
            btn_normal.Click += new EventHandler(normalToolStripMenuItem_Click);
            btn_normal.Click += new EventHandler(formKapatma);
            btn_zor.Click += new EventHandler(zorToolStripMenuItem_Click);
            btn_zor.Click += new EventHandler(formKapatma);
            btn_extreme.Click += new EventHandler(extremeToolStripMenuItem_Click);
            btn_extreme.Click += new EventHandler(formKapatma);

            frm.Controls.Add(lbl_bitti);
            frm.Controls.Add(btn_kolay);
            frm.Controls.Add(btn_normal);
            frm.Controls.Add(btn_zor);
            frm.Controls.Add(btn_extreme);
            frm.Controls.Add(lbl_seviye);
            frm.Controls.Add(pnl_bitisResmi);
        }

        void formKapatma(object sender, EventArgs e)
        {
            frm.Close();
        }

        void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;
        }

        void ipucu_Paint(object sender, PaintEventArgs e)
        {
            g = ipucu.CreateGraphics();
            g.DrawImage(resizeImage(anaResim, new Size(zorluk * 100, zorluk * 100)), new Rectangle(0, 0, zorluk * 100, zorluk * 100));
        }       

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            frm = new Form();
            frm.SetBounds(100, 100, 200, 220);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.MaximizeBox = false;
            frm.MinimizeBox = false;
            frm.Text = "Oyun Bitti";
            frm.TopMost = true;
            this.Enabled = false;
            frm.Load += new EventHandler(frm_Load);
            frm.Show();
            frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
        }
        
    }
}
