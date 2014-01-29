using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace puzzle2
{
    class Tas
    {
        public int posX;
        public int posY;
        private Image img;
        public Boolean bosMu = false;
        public int index;

        public Tas(int posiX, int posiY, Image imag, int indis) 
        {
            this.posX = posiX;
            this.posY = posiY;
            this.img = imag;
            this.index = indis;
        }

        public void Ciz()
        { 
            Form1.g.DrawImage(img, new Rectangle(posX * 110, posY * 110, 100, 100));             
        }
    }
}
