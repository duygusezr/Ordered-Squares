namespace düğme_oyunu
{
    public partial class Form1 : Form
    {
        private Button[,] buttons; // Düğmeleri tutan 2D dizi
        private int gridSize = 5; // 5x5'lik bir ızgara
        private Button emptyButton; // Boş alanı temsil eden düğme

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.ClientSize = new Size(gridSize * 60, gridSize * 60);

            Load += Form1_Load; // Form yüklendiğinde çalışacak olayı bağla
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buttons = new Button[gridSize, gridSize];
            int[] numbers = Enumerable.Range(1, 24).ToArray();
            Shuffle(numbers); // Düğme numaralarını karıştır

            int k = 0;
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].Size = new Size(60, 60);
                    buttons[i, j].Location = new Point(j * 60, i * 60);
                    buttons[i, j].Click += new EventHandler(Button_Click);
                    this.Controls.Add(buttons[i, j]);

                    if (k < 24)
                    {
                        buttons[i, j].Text = numbers[k].ToString();
                        k++;
                    }
                    else
                    {
                        buttons[i, j].Text = "";
                        emptyButton = buttons[i, j]; // Boş alanı belirle
                    }
                }
            }
        }

        private void Shuffle(int[] array)
        {
            Random rand = new Random();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            // Tıklanan düğme boş alanın dört komşusundan biri mi veya çaprazında mı?
            if (IsAdjacentOrDiagonal(btn))
            {
                // Düğmenin text değerini boş düğmeye atayalım, boş düğmenin text değerini boş yapalım
                emptyButton.Text = btn.Text;
                btn.Text = "";

                // Yeni boş alanı belirleyelim
                emptyButton = btn;

                // Oyunun bitip bitmediğini kontrol et
                CheckIfSolved();
            }
        }

        private bool IsAdjacentOrDiagonal(Button btn)
        {
            // Tıklanan düğmenin ve boş düğmenin koordinatlarını alalım
            int btnRow = btn.Location.Y / 60;
            int btnCol = btn.Location.X / 60;
            int emptyRow = emptyButton.Location.Y / 60;
            int emptyCol = emptyButton.Location.X / 60;

            // Tıklanan düğmenin boş düğmenin üstünde, altında, solunda, sağında veya çaprazında olup olmadığını kontrol et
            bool isAbove = (btnRow == emptyRow - 1) && (btnCol == emptyCol);
            bool isBelow = (btnRow == emptyRow + 1) && (btnCol == emptyCol);
            bool isLeft = (btnRow == emptyRow) && (btnCol == emptyCol - 1);
            bool isRight = (btnRow == emptyRow) && (btnCol == emptyCol + 1);

            // Çaprazlar için kontrol
            bool isTopLeftDiagonal = (btnRow == emptyRow - 1) && (btnCol == emptyCol - 1);
            bool isTopRightDiagonal = (btnRow == emptyRow - 1) && (btnCol == emptyCol + 1);
            bool isBottomLeftDiagonal = (btnRow == emptyRow + 1) && (btnCol == emptyCol - 1);
            bool isBottomRightDiagonal = (btnRow == emptyRow + 1) && (btnCol == emptyCol + 1);

            return isAbove || isBelow || isLeft || isRight || isTopLeftDiagonal || isTopRightDiagonal || isBottomLeftDiagonal || isBottomRightDiagonal;
        }

        private void CheckIfSolved()
        {
            int k = 1;
            bool solved = true;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (buttons[i, j] != emptyButton && buttons[i, j].Text != k.ToString())
                    {
                        solved = false;
                        break;
                    }
                    k++;
                }
            }

            if (solved)
            {
                MessageBox.Show("Tebrikler! Oyunu kazandınız!");
            }
        }
    }
}
