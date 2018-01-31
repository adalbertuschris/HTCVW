using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manual
{
    public partial class Form1 : Form
    {
        const string sw1 = "1. Zmiana orientacji modelu";
        const string sw2 = "2. Zmiana pozycji modelu";
        const string sw3 = "3. Skalowanie modelu";
        const string sw5 = "4. Włączanie/Wyłączanie trybu roboczego";
        const string wSw1 = "5. Zmiana stylu wyświetlania całego modelu";
        const string wSw2 = "6. Przechodzenie do kolejnego elementu modelu";
        const string wSw3 = "7. Przechodzenie do poprzedniego elementu modelu";
        const string wSw4 = "8. Zmiana stylu wyświetlania dla wybranego elementu";

        List<string> text = new List<string> { sw1, sw2, sw3, sw5, wSw1, wSw2, wSw3, wSw4 };
        Dictionary<string, Bitmap> resource = new Dictionary<string, Bitmap>()
        {
            { sw1, Images.orientacja },
            { sw2, Images.pozycjonowanie },
            { sw3, Images.skalowanie },
            { sw5, Images.w5 },
            { wSw1, Images.w1 },
            { wSw2, Images.w2 },
            { wSw3, Images.w3 },
            { wSw4, Images.w4_2 },
        };

        int counter = 0;

        public Form1()
        {
            InitializeComponent();
        }

        public void Next()
        {
            counter++;
            if (counter == 8)
            {
                counter = 0;
            }
            string key = text[counter];
            textBox1.Text = key;
            pictureBox1.Image = resource[key];            
        }

        public void Previous()
        {
            counter--;
            if (counter == -1)
            {
                counter = 7;
            }
            string key = text[counter];
            textBox1.Text = key;
            pictureBox1.Image = resource[key];            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string key = text[counter];
            textBox1.Text = key;
            pictureBox1.Image = resource[key];
        }

       
       

        //private void btnNext_Paint(object sender, PaintEventArgs e)
        //{
        //    Next();
        //}

        
        private void btnNext_Click(object sender, EventArgs e)
        {
            Next();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            Previous();
        }
    }
}
