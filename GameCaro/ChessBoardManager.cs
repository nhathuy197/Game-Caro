using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;


namespace GameCaro
{
    public class ChessBoardManager
    {
        //lay ham tu form
        #region Propertices
        private Panel chessBoard;
        public Panel ChessBoard
        {
            get { return chessBoard; }
            set { chessBoard = value; }
        }

        public Point Location { get; private set; }
        public List<Player> Player { get => _player; set => _player = value; }
        public int CurrentPlayer { get => currentPlayer; set => currentPlayer = value; }
        public TextBox PlayerName1 { get => PlayerName; set => PlayerName = value; }
        public PictureBox PlayerMask { get => PlayerMark; set => PlayerMark = value; }
        public List<List<Button>> Matrix { get => matrix; set => matrix = value; }

        private int currentPlayer;
        private TextBox PlayerName;
        private PictureBox PlayerMark;
        private List<List<Button>> matrix;
       
        #endregion

        #region Initialize
        public ChessBoardManager(Panel chessBoard,TextBox playerName, PictureBox mark)
        {
            this.ChessBoard = chessBoard;
            this.PlayerName = playerName;
            this.PlayerMask = mark;
            this.Player = new List<Player>()
            {
                new Player("Huy",Image.FromFile(Application.StartupPath + "\\Resources\\x.png")),
                new Player("Ho",Image.FromFile(Application.StartupPath + "\\Resources\\o.png"))
            };
            CurrentPlayer = 0;
            ChangePlayer();
        }
        #endregion
        private List<Player> _player;
        #region Methods

        public void DrawChessBoard()
        {
            ChessBoard.Enabled = true;
            Matrix = new List<List<Button>>();

            //Tao mot btn chua vi tri cua btn truoc do
            Button oldButton = new Button() { Width = 0, Location = new Point(0, 0) };

            for (int i=0;i<Chess.chess_board_width;i++)
            {
                Matrix.Add(new List<Button>());
                for (int j=0; j<=Chess.chess_board_height;j++)
                {
                    Button btntemp = new Button()
                    {
                        Width = Chess.chess_width,
                        Height = Chess.chess_height,
                        Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString()
                    };
                    btntemp.Click += Btntemp_click;
                    ChessBoard.Controls.Add(btntemp);
                    Matrix[i].Add(btntemp);
                    oldButton = btntemp;
                }
                oldButton.Location = new Point(0, oldButton.Location.Y + Chess.chess_height);
                oldButton.Width = 0;
                oldButton.Height = 0;
            }

        }
       
        void Btntemp_click(Object sender,EventArgs e)
        {
            Button btntemp = sender as Button;
            if (btntemp.BackgroundImage != null) { return;  }
            Mask(btntemp);
            ChangePlayer();
    
        }
        private void EndGame()
        {
            MessageBox.Show("Kết thúc !!!");
        }
        private bool IsEndGame(Button btntemp)
        {
            return IsEndHorizontal(btntemp)||IsEndVertical(btntemp)||IsEndPrimary(btntemp)||IsEndSub(btntemp);
        }
        private Point GetChessPoint(Button btn)
        {   
            int vertical = Convert.ToInt32(btn.Tag);
            int horizontal = Matrix[vertical].IndexOf(btn);
            Point point = new Point(horizontal,vertical);
            return point;
        }
        private bool IsEndHorizontal(Button btntemp)
        {
            Point point = GetChessPoint(btntemp);
            int CountLeft = 0;
            //Xét trái
            for(int i = point.X; i >= 0 ; i--)
            {
                if (Matrix[point.Y][i].BackgroundImage == btntemp.BackgroundImage)
                {
                    CountLeft++;
                }
                else break;
            }
            //Xét phải
            int CountRight = 0;
            for(int i=point.X+1;i < Chess.chess_board_width; i++)
            {
                if (Matrix[point.Y][i].BackgroundImage == btntemp.BackgroundImage)
                {
                    CountRight++;
                }
                else break;
            }
            return CountLeft+CountRight==5;
        }
        private bool IsEndVertical(Button btntemp)
        {
            Point point = GetChessPoint(btntemp);
            int CountTop = 0;
            //Xét trên
            for (int i = point.Y; i >= 0; i--)
            {
                if (Matrix[i][point.X].BackgroundImage == btntemp.BackgroundImage)
                {
                    CountTop++;
                }
                else break;
            }
            //Xét dưới
            int CountBottom = 0;
            for (int i = point.Y+1; i < Chess.chess_board_height; i++)
            {
                if (Matrix[i][point.X].BackgroundImage == btntemp.BackgroundImage)
                {
                    CountBottom++;
                }
                else break;
            }
            return CountTop + CountBottom == 5;
        }
        private bool IsEndPrimary(Button btntemp)
        {
            Point point = GetChessPoint(btntemp);
            int CountTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0) break;
                if (Matrix[point.Y-i][point.X-i].BackgroundImage == btntemp.BackgroundImage)
                {
                    CountTop++;
                }
                else break;
            }
            int CountBottom = 0;
            for (int i = 1; i <= Chess.chess_board_width-point.X; i++)
            {
                if (point.Y + i >= Chess.chess_board_height || point.X + i > Chess.chess_board_width) break;
                if (Matrix[point.Y + i][point.X + i].BackgroundImage == btntemp.BackgroundImage)
                {
                    CountBottom++;
                }
                else break;
            }
            return CountTop + CountBottom == 5;
        }
        private bool IsEndSub(Button btntemp)
        {
            Point point = GetChessPoint(btntemp);
            int CountTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X + i > Chess.chess_board_width || point.Y - i < 0) break;
                if (Matrix[point.Y - i][point.X + i].BackgroundImage == btntemp.BackgroundImage)
                {
                    CountTop++;
                }
                else break;
            }
            int CountBottom = 0;
            for (int i = 1; i <= Chess.chess_board_width - point.X; i++)
            {
                if (point.Y + i >= Chess.chess_board_height || point.X + i < 0) break;
                if (Matrix[point.Y + i][point.X - i].BackgroundImage == btntemp.BackgroundImage)
                {
                    CountBottom++;
                }
                else break;
            }
            return CountTop + CountBottom == 5;
        }
        private void Mask(Button btntemp)
        {
            btntemp.BackgroundImage = Player[CurrentPlayer].Mark;
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
        }
        private void ChangePlayer()
        {
            PlayerName.Text = Player[CurrentPlayer].Name;
            PlayerMark.Image = Player[CurrentPlayer].Mark;
        }        
        
        #endregion
    }
}
