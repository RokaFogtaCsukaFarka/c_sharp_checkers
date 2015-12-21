using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using System.IO;
using sfx3nr.CheckersDataSetTableAdapters;
using System.Runtime.Serialization;

namespace sfx3nr
{
    /// <summary>
    /// Description of Form1.
    /// </summary>
    public partial class GameForm : Form
    {
        public string usernameCurrent;

        public Logger logger;

        private gameTable table;

        //if false player comes, else computer comes
        public Boolean moveStatus;
        public Boolean won;
        public string whoWon;

        private DataTable userTable;
        private UsersTableAdapter dataAdapter;
        private DataRow user;

        public GameForm(DataTable userTable, UsersTableAdapter dataAdapter, DataRow user)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // Add constructor code after the InitializeComponent() call.
            //
            this.DataAdapter = dataAdapter;
            this.UserTable = userTable;
            this.User = user;

            logger = new Logger(true);
           // this.error_label.Text = logger.fullFilePath;

            this.username_label.Text = user.Field<string>("Name");

            Table = new gameTable(this, logger);

            moveStatus = false;
            won = false;
            whoWon = "none";
        }

        public gameTable Table
        {
            get
            {
                return table;
            }

            set
            {
                table = value;
            }
        }

        public DataRow User
        {
            get
            {
                return this.user;
            }

            set
            {
                this.user = value;
            }
        }

        public DataTable UserTable
        {
            get
            {
                return userTable;
            }

            set
            {
                userTable = value;
            }
        }

        public UsersTableAdapter DataAdapter
        {
            get
            {
                return dataAdapter;
            }

            set
            {
                dataAdapter = value;
            }
        }

        private void next_step_ok_button_Click(object sender, EventArgs e)
        {
            string[] input = this.next_step_textBox.Text.Split(':');
            var move = new KeyValuePair<string, string>(input[0], input[1]);

            string loggerString = "";
            for(int i = 7; i > -1; i--)
            {
                for (int j = 0; j < 8; j++)
                    loggerString += Table.Table[i*8 + j].Item1 + " ";
                logger.Write(loggerString);
                loggerString = "";
            }

            Table.gamePlayer.movePuppet(move);
            if (moveStatus) 
                Table.Computer.movePuppet();
        }

        public class gameTable
        {
            //In this array I will represent the Game:
            // Eg. 101 - the first bit is given to represent the status (0 - normal, 1 - king)
            //         - the second bit is to represent colour (0 - black, 1 - white)
            //         - the third bit is given to represent if there is any puppet there
            public GameForm gameForm_;
            private List<Tuple<string, PictureBox>> table;
            private Logger logger;

            //winningStatus = 0 if black loses
            //                1 if unpartial
            //                2 if black wins
            public int winningStatus = 1;
            public blackPlayer gamePlayer;
            public whitePlayer Computer;
            public Boolean moveIsEnabled;
            private List<Tuple<string, string, string>> moves;
            private int noOfMoves;

            public gameTable(GameForm gameForm, Logger logger)
            {
                gameForm_ = gameForm;
                Table = new List<Tuple<string, PictureBox>>();

                this.logger = logger;

                //Table representation starting from the a1 ending with h8
                Table.Add(new Tuple<string, PictureBox>("01101", gameForm_.pictureBox1)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("01102", gameForm_.pictureBox2)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("01103", gameForm_.pictureBox3)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("01104", gameForm_.pictureBox4)); Table.Add(new Tuple<string, PictureBox>("00000", null));
                Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("01105", gameForm_.pictureBox5)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("01106", gameForm_.pictureBox6)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("01107", gameForm_.pictureBox7)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("01108", gameForm_.pictureBox8));
                Table.Add(new Tuple<string, PictureBox>("01109", gameForm_.pictureBox9)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("01110", gameForm_.pictureBox10)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("01111", gameForm_.pictureBox11)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("01112", gameForm_.pictureBox12)); Table.Add(new Tuple<string, PictureBox>("00000", null));
                Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00000", null));
                Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00000", null));
                Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00113", gameForm_.pictureBox13)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00114", gameForm_.pictureBox14)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00115", gameForm_.pictureBox15)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00116", gameForm_.pictureBox16));
                Table.Add(new Tuple<string, PictureBox>("00117", gameForm_.pictureBox17)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00118", gameForm_.pictureBox18)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00119", gameForm_.pictureBox19)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00120", gameForm_.pictureBox20)); Table.Add(new Tuple<string, PictureBox>("00000", null));
                Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00121", gameForm_.pictureBox21)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00122", gameForm_.pictureBox22)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00123", gameForm_.pictureBox23)); Table.Add(new Tuple<string, PictureBox>("00000", null)); Table.Add(new Tuple<string, PictureBox>("00124", gameForm_.pictureBox24));
                gamePlayer = new blackPlayer(gameForm, this, logger);
                Computer = new whitePlayer(gameForm, this, logger);
                this.Moves = new List<Tuple<string, string, string>>();
                NoOfMoves = 0;

                moveIsEnabled = true;
            }

            public List<Tuple<string, PictureBox>> Table
            {
                set { table = value; }
                get { return table; }
            }

            public List<Tuple<string, string, string>> Moves
            {
                get
                {
                    return moves;
                }
                set
                {
                    moves = value;
                }
            }

            public int NoOfMoves
            {
                get { return noOfMoves; }
                set { noOfMoves = value; }
            }

            public void renderPuppet(int[] position, Tuple<string, PictureBox> puppet)
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
                puppet.Item2.Location = new Point(position[0], position[1]);
                //If king should render King image.
                if ((Int32.Parse(puppet.Item1.Substring(0, 3)) & 101) == 101)
                    puppet.Item2.Image = ((Image)(resources.GetObject("checkers_white_king.Image")));
                else if ((Int32.Parse(puppet.Item1.Substring(0, 3)) & 101) == 111)
                    puppet.Item2.Image = ((Image)(resources.GetObject("checkers_white_king.Image")));
            }

            public void renderTable()
            {
                // Render the screen against the array
                int[] position = { 0, 0 };
                Tuple<string, PictureBox> puppet = new Tuple<string, PictureBox>("00000", null);

                //The rendering pathway is the way the numbers are evaluating on the table
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (Table[i * 8 + j].Item2 != null)
                        {
                            position[0] = 35 + j * 75;
                            position[1] = 560 - i * 75;
                            puppet = Table[i * 8 + j];
                            renderPuppet(position, puppet);
                        }
            }

            public abstract class Player
            {
                private GameForm gameForm;
                private Logger logger;
                private gameTable gameTable;

                public Boolean canBeat;
                public Boolean canMove;
                public Int32[] whichCanBeat;

                public GameForm GameForm { get { return this.gameForm; } set { this.gameForm = value; } }
                public Logger Logger { get { return this.logger; } set { this.logger = value; } }
                public gameTable GameTable { get { return this.gameTable; } set { this.gameTable = value; } }

                public Player() { }
                public Player(GameForm gameForm, gameTable table, Logger logger)
                {
                    GameForm = gameForm;
                    Logger = logger;
                    gameTable = table;
                    canBeat = false;
                    whichCanBeat = new Int32[64];
                }

                public Boolean goingToBeat(Int16 fromNumInt, Int16 toNumInt)
                {
                    if (((toNumInt - fromNumInt) == 18) || ((toNumInt - fromNumInt) == 14) || ((toNumInt - fromNumInt) == -18) || ((toNumInt - fromNumInt) == -14))
                        return true;
                    else
                        return false;
                }

                public abstract void playerCanBeat(out Int32[] whichCanBeatLoc);
                public abstract void changeToBeAKing(Int16 place);

                public virtual void movePuppet() { }
                public virtual void movePuppet(KeyValuePair<string, string> move) { }
            }

            public class EmptyPlaceExeption : Exception, ISerializable
            {
                public EmptyPlaceExeption() { }

                public EmptyPlaceExeption(string message) { }

                protected EmptyPlaceExeption(SerializationInfo info, StreamingContext context) { }

            }

            public class UnavailableMoveException : Exception, ISerializable
            {
                public UnavailableMoveException() { }

                public UnavailableMoveException(string message) { }

                protected UnavailableMoveException(SerializationInfo info, StreamingContext context) { }
            }

            public class BeatAvailableException : Exception, ISerializable
            {
                public BeatAvailableException() { }

                public BeatAvailableException(string message) { }

                protected BeatAvailableException(SerializationInfo info, StreamingContext context) { }

            }

            public class blackPlayer : gameTable.Player
            {
                private GameForm gameForm;
                private gameTable gameTable;
                private Logger logger;

                public blackPlayer() { }
                public blackPlayer(GameForm gameForm, gameTable gameTable, Logger logger)
                {
                    this.gameForm = gameForm;
                    this.gameTable = gameTable;
                    this.logger = logger;
                    this.canMove = true;
                }

                public Boolean hasWon()
                {
                    String[] numberStringArray = new String[12] { "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" };

                    for (int i = 0; i < 64; i++)
                    {
                        for (int j = 0; j < 12; j++)
                        {
                            string elementNumber = gameTable.Table[i].Item1.Substring(3, 2);
                            if ( !canMove || String.Compare(elementNumber, numberStringArray[j]) == 0 )
                                return false;
                        }
                    }

                    gameForm.won = true;
                    gameForm.whoWon = "black";
                    logger.Write("The black player has won!");
                    gameForm.error_label.Text = "The black player has won!";

                    if (String.Compare(gameForm.whoWon, "black") == 0)
                    {
                        int? value = gameForm.User.Field<int?>("Highscore");
                        gameForm.User.SetField<int?>("Highscore", (int)value + 400);
                    }

                    gameForm.dataAdapter.Update(gameForm.User);

                    return true;
                }

                public void convertMoveToIntValues(KeyValuePair<string, string> move, ref Int16 fromNumInt, ref Int16 toNumInt)
                {
                    Byte[] fromNumTemp = { 0, 0 };
                    Byte[] fromNum = { 0, 0, 0, 0 };
                    fromNumTemp = Encoding.ASCII.GetBytes(move.Key);
                    fromNumTemp[0] -= 97;
                    fromNumTemp[1] -= 49;
                    fromNum[0] = fromNumTemp[0];
                    fromNum[2] = fromNumTemp[1];
                    fromNumInt = (Int16)(BitConverter.ToInt16(fromNum, 2) * 8 + BitConverter.ToInt16(fromNum, 0));

                    Byte[] toNumTemp = { 0, 0 };
                    Byte[] toNum = { 0, 0, 0, 0 };
                    toNumTemp = Encoding.ASCII.GetBytes(move.Value);
                    toNumTemp[0] -= 97;
                    toNumTemp[1] -= 49;
                    toNum[0] = toNumTemp[0];
                    toNum[2] = toNumTemp[1];
                    toNumInt = (Int16)(BitConverter.ToInt16(toNum, 2) * 8 + BitConverter.ToInt16(toNum, 0));
                }

                //This method returns which puppet from the white armade can beat any opponent
                public override void playerCanBeat(out Int32[] whichCanBeatLoc)
                {
                    Boolean canBeat = false;
                    Int32 j = 0;
                    var whichCanBeat = new Int32[64];
                    whichCanBeatLoc = new Int32[64];
                    try
                    {
                        for (int i = 0; i < 64; i++)
                        {
                            var tuple = gameTable.Table[i];

                            if (String.Compare(tuple.Item1.Substring(0, 3), "000") == 0)
                                continue;
                            //If the puppet is not a king and there is a beatable enemy against it 
                            else if ((String.Compare(tuple.Item1.Substring(0, 3), "011") == 0) &&
                                    (((i < 46) && !((i % 8 == 6) || (i % 8 == 7)) && (String.Compare(gameTable.Table[i + 9].Item1.Substring(0, 3), "001") == 0 || String.Compare(gameTable.Table[i + 9].Item1.Substring(0, 3), "101") == 0) && (String.Compare(gameTable.Table[i + 18].Item1.Substring(0, 3), "000") == 0)) ||
                                    ((i < 48) && !((i % 8 == 0) || (i % 8 == 1)) && (String.Compare(gameTable.Table[i + 7].Item1.Substring(0, 3), "001") == 0 || String.Compare(gameTable.Table[i + 7].Item1.Substring(0, 3), "101") == 0) && (String.Compare(gameTable.Table[i + 14].Item1.Substring(0, 3), "000") == 0))))
                            {
                                whichCanBeat[j] = i;
                                canBeat = true;
                                j++;
                            }
                            //If the puppet is a king and there is a beatable enemy against it
                            else if ((String.Compare(tuple.Item1.Substring(0, 3), "111") == 0) &&
                                ((((i > 15) && !((i % 8 == 0) || (i % 8 == 1)) && ((String.Compare(gameTable.Table[i - 7].Item1.Substring(0, 3), "001") == 0 || String.Compare(gameTable.Table[i - 7].Item1.Substring(0, 3), "101") == 0)) && String.Compare(gameTable.Table[i - 14].Item1.Substring(0, 3), "000") == 0)) ||
                                (((i > 17) && !((i % 8 == 6) || (i % 8 == 7)) && ((String.Compare(gameTable.Table[i - 9].Item1.Substring(0, 3), "001") == 0 || String.Compare(gameTable.Table[i - 9].Item1.Substring(0, 3), "101") == 0)) && String.Compare(gameTable.Table[i - 18].Item1.Substring(0, 3), "000") == 0)) ||
                                (((i < 46) && !((i % 8 == 6) || (i % 8 == 7)) && ((String.Compare(gameTable.Table[i + 9].Item1.Substring(0, 3), "001") == 0 || String.Compare(gameTable.Table[i + 9].Item1.Substring(0, 3), "101") == 0)) && String.Compare(gameTable.Table[i + 18].Item1.Substring(0, 3), "000") == 0)) ||
                                (((i < 48) && !((i % 8 == 0) || (i % 8 == 1)) && ((String.Compare(gameTable.Table[i + 7].Item1.Substring(0, 3), "001") == 0 || String.Compare(gameTable.Table[i + 7].Item1.Substring(0, 3), "101") == 0)) && String.Compare(gameTable.Table[i + 14].Item1.Substring(0, 3), "000") == 0))))
                            {
                                whichCanBeat[j] = i;
                                canBeat = true;
                                j++;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        goto continueLabel;
                    }

                continueLabel:
                    if (!canBeat)
                    {
                        this.canBeat = false;
                        whichCanBeat[0] = -1;
                        Array.Copy(whichCanBeat, whichCanBeatLoc, 64);
                    }
                    else
                    {
                        this.canBeat = true;
                        whichCanBeat[j] = -1;
                        Array.Copy(whichCanBeat, whichCanBeatLoc, 64);
                    }
                }

                public override void changeToBeAKing(Int16 place)
                {
                    var tuple = gameTable.Table[place];
                    gameTable.Table[place] = new Tuple<string, PictureBox>("111" + tuple.Item1.Substring(3, 2), tuple.Item2);
                    tuple.Item2.Image = Properties.Resources.checkers_black_king;
                }

                //We should represent the table.Table in an Array, and thus move the puppets on the board - the player is with the white
                public override void movePuppet(KeyValuePair<string, string> move)
                {
                    try
                    {
                        Int16 fromNumInt = 0;
                        Int16 toNumInt = 0;
                        Regex regex = new Regex("[a-h][1-9]:[a-h][1-9]");
                        if (regex.IsMatch(move.Key + ":" + move.Value))
                            this.convertMoveToIntValues(move, ref fromNumInt, ref toNumInt);
                        else
                            throw new UnavailableMoveException("Mistype!");

                        logger.Write("Black:");

                        if (!hasWon())
                        {

                            //If we want to move a puppet that does not exists in the current place.
                            playerCanBeat(out this.whichCanBeat);

                            string canBeatConcat = "";
                            logger.Write("CanBeat:" + canBeat);
                            for (int i = 0; whichCanBeat[i] != -1; i++)
                            {
                                canBeatConcat += whichCanBeat[i].ToString() + " ";
                            }
                            logger.Write(canBeatConcat);
                            var tuple = gameTable.Table[fromNumInt];

                            if (this.canBeat)
                            {
                                if (goingToBeat(fromNumInt, toNumInt))
                                {
                                    logger.Write("Going to beat: true");

                                    gameTable.Table[fromNumInt] = gameTable.Table[toNumInt];

                                    if (toNumInt - fromNumInt == 18)
                                    {
                                        gameTable.Table[toNumInt - 9].Item2.Hide();
                                        gameTable.Table[toNumInt - 9] = new Tuple<string, PictureBox>("00000", null);
                                    }
                                    else if (toNumInt - fromNumInt == 14)
                                    {
                                        gameTable.Table[toNumInt - 7].Item2.Hide();
                                        gameTable.Table[toNumInt - 7] = new Tuple<string, PictureBox>("00000", null);
                                    }

                                    gameTable.Table[toNumInt] = tuple;

                                    //if the puppet is on the last row at his opponent, then change it to be a king
                                    if (tuple.Item1.Substring(0, 3) == "011" && toNumInt > 55)
                                    {
                                        changeToBeAKing(toNumInt);
                                        logger.Write(toNumInt + " puppet is now a king.");
                                        gameForm.previous_step_listBox.Items.Add(toNumInt + " puppet is now a king.");
                                    }

                                    gameTable.renderTable();

                                    gameForm.moveStatus = true;

                                    logger.Write("MoveStatus: " + gameForm.moveStatus);
                                }
                                else
                                    throw new BeatAvailableException("A beat is available, please look for it!");
                            }
                            else
                            {
                                if (String.Compare(tuple.Item1.Substring(0, 3), "000") == 0)
                                    throw new EmptyPlaceExeption("There is no puppet on the selected field. Please choose another!");
                                //If the puppet is not a king and there is no puppet on its target place and wants to move other than diagonally forward
                                else if (((String.Compare(tuple.Item1.Substring(0, 3), "011") == 0) && (String.Compare(gameTable.Table[toNumInt].Item1.Substring(0, 3), "001") > 0) || String.Compare(gameTable.Table[toNumInt].Item1.Substring(0, 3), "101") > 0) &&
                                    !((toNumInt - fromNumInt) == 9 || (toNumInt - fromNumInt) == 7))
                                    throw new UnavailableMoveException("There is not such move for a normal puppet. Try again diagonally, and forward!");
                                // Else if the puppet to be moved is a king and there is no beatable enemy againts it and wants to move other than diagonally
                                else if ((tuple.Item1.Substring(0, 3) == "111") && !(!(((toNumInt - fromNumInt) == 9) || (toNumInt - fromNumInt) == 7) ||
                                    !((toNumInt - fromNumInt) == -9) || ((toNumInt - fromNumInt) == -7)))
                                    throw new UnavailableMoveException("There is not such move for a king puppet. Try again diagonally, and forward!");
                                // Move puppet in the array
                                else
                                {
                                    logger.Write("Going to beat : false");
                                    gameTable.Table[fromNumInt] = gameTable.Table[toNumInt];
                                    gameTable.Table[toNumInt] = tuple;
                                    tuple = gameTable.Table[toNumInt];
                                    //If it's a beat we should remove the interceeding puppet
                                    if (toNumInt - fromNumInt == 18)
                                    {
                                        gameTable.Table[fromNumInt + 9].Item2.Hide();
                                        gameTable.Table[fromNumInt + 9] = new Tuple<string, PictureBox>("00000", null);
                                    }
                                    else if (toNumInt - fromNumInt == 14)
                                    {
                                        gameTable.Table[fromNumInt + 7].Item2.Hide();
                                        gameTable.Table[fromNumInt + 7] = new Tuple<string, PictureBox>("00000", null);
                                    }

                                    //if the puppet is on the last row at his opponent, then change it to be a king
                                    if (tuple.Item1.Substring(0, 3) == "011" && toNumInt > 55)                                    {
                                        changeToBeAKing(toNumInt);
                                        logger.Write(toNumInt + " puppet is now a king.");
                                        gameForm.previous_step_listBox.Items.Add(toNumInt + " puppet is now a king.");
                                    }

                                    gameTable.renderTable();

                                    gameForm.moveStatus = true;

                                    hasWon();

                                    logger.Write("MoveStatus: " + gameForm.moveStatus + "\n");
                                }
                            }

                            gameTable.Moves.Add(new Tuple<string, string, string>("black", move.Key, move.Value));
                            gameForm.previous_step_listBox.Items.Add(gameTable.Moves[gameTable.NoOfMoves].Item1 + " - " + gameTable.Moves[gameTable.NoOfMoves].Item2 + ":" + gameTable.Moves[gameTable.NoOfMoves].Item3);
                            if (canBeat)
                                gameForm.previous_step_listBox.Items.Add("BEAT");
                            gameTable.NoOfMoves++;
                            logger.Write("black - " + move.Key + ":" + move.Value + " -> " + fromNumInt + ":" + toNumInt + "\n");
                        }
                    }

                    catch (Exception e)
                    {
                        logger.Write(e.ToString());
                        logger.Write("MoveStatus: false \n");
                        gameForm.error_label.Text = e.ToString();
                        gameForm.moveStatus = false;
                    }

                }
            }

            public class whitePlayer : gameTable.Player
            {
                private GameForm gameForm;
                private gameTable gameTable;
                private Logger logger;
                private Int32[] whichPuppetCanMove;

                public whitePlayer() { }

                public whitePlayer(GameForm gameForm, gameTable gameTable, Logger logger)
                {
                    this.gameForm = gameForm;
                    this.gameTable = gameTable;
                    this.logger = logger;
                    this.whichPuppetCanMove = new Int32[12];
                }

                public Boolean hasWon()
                {
                    String[] numberStringArray = new String[12] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };

                    for (int i = 0; i < 64; i++)
                    {
                        for (int j = 0; j < 12; j++)
                        {
                            String elementNumber = gameTable.table[i].Item1.Substring(3, 2);
                            if (!canMove || String.Compare(elementNumber, numberStringArray[j]) == 0)
                                return false;
                        }
                    }

                    gameForm.won = true;
                    gameForm.whoWon = "white";
                    gameForm.error_label.Text = "The white player has won!";

                    if (String.Compare(gameForm.whoWon, "white") == 0)
                    {
                        int? value = gameForm.User.Field<int?>("Highscore");
                        gameForm.User.SetField<int?>("Highscore", (int)value - 200);
                    }
                    //Why is this null? Shall I give the GameForm the dataadapter, as well as the dataTable as parameter?
                    gameForm.dataAdapter.Update(gameForm.User);

                    return true;
                }

                public KeyValuePair<string, string> convertIntValuesToMove(Int16 fromNumInt, Int16 toNumInt)
                {

                    Int32[] fromNumArray = new Int32[2] { fromNumInt % 8 + 97, (int)Math.Floor((double)(fromNumInt / 8)) + 49 };
                    char keyOne = (char)fromNumArray[0];
                    char keyTwo = (char)fromNumArray[1];
                    string key = String.Concat(keyOne.ToString(), keyTwo.ToString());

                    Int32[] toNumArray = new Int32[2] { toNumInt % 8 + 97, (int)Math.Floor((double)(toNumInt / 8)) + 49 };
                    char valueOne = (char)toNumArray[0];
                    char valueTwo = (char)toNumArray[1];
                    string value = String.Concat(valueOne.ToString(), valueTwo.ToString());

                    return new KeyValuePair<string, string>(key, value);
                }

                //This method returns which puppet from the white armade can beaet any opponent
                public override void playerCanBeat(out Int32[] whichCanBeatLoc)
                {
                    Boolean canBeat = false;
                    Int32 j = 0;
                    Int32[] whichCanBeat = new Int32[64];
                    whichCanBeatLoc = new Int32[64];

                    try
                    {
                        for (int i = 0; i < 64; i++)
                        {
                            var tuple = gameTable.Table[i];

                            if (String.Compare(tuple.Item1.Substring(0, 3), "000") == 0)
                                continue;
                            //If the puppet is not a king and there is a beatable enemy against it 
                            else if ((String.Compare(tuple.Item1.Substring(0, 3), "001") == 0) &&
                                (((i > 17) && !((i % 8 == 0) || (i % 8 == 1)) && (String.Compare(gameTable.Table[i - 9].Item1.Substring(0, 3), "011") == 0 || String.Compare(gameTable.Table[i - 9].Item1.Substring(0, 3), "111") == 0) && (String.Compare(gameTable.Table[i - 18].Item1.Substring(0, 3), "000") == 0)) ||
                                ((i > 15) && !((i % 8 == 6) || (i % 8 == 7)) && (String.Compare(gameTable.Table[i - 7].Item1.Substring(0, 3), "011") == 0 || String.Compare(gameTable.Table[i - 7].Item1.Substring(0, 3), "111") == 0) && (String.Compare(gameTable.Table[i - 14].Item1.Substring(0, 3), "000") == 0))))
                            {
                                whichCanBeat[j] = i;
                                canBeat = true;
                                j++;
                            }
                            //If the puppet is a king and there is a beatable enemy against it
                            else if ((String.Compare(tuple.Item1.Substring(0, 3), "101") == 0) &&
                                ((((i > 15) && !((i % 8 == 6) || (i % 8 == 7)) && (String.Compare(gameTable.Table[i - 7].Item1.Substring(0, 3), "011") == 0 || String.Compare(gameTable.Table[i - 7].Item1.Substring(0, 3), "111") == 0) && String.Compare(gameTable.Table[i - 14].Item1.Substring(0, 3), "000") == 0)) ||
                                (((i > 17) && !((i % 8 == 0) || (i % 8 == 1)) && (String.Compare(gameTable.Table[i - 9].Item1.Substring(0, 3), "011") == 0 || String.Compare(gameTable.Table[i - 9].Item1.Substring(0, 3), "111") == 0) && String.Compare(gameTable.Table[i - 18].Item1.Substring(0, 3), "000") == 0)) ||
                                (((i < 46) && !((i % 8 == 6) || (i % 8 == 7)) && (String.Compare(gameTable.Table[i + 9].Item1.Substring(0, 3), "011") == 0 || String.Compare(gameTable.Table[i + 9].Item1.Substring(0, 3), "111") == 0) && String.Compare(gameTable.Table[i + 18].Item1.Substring(0, 3), "000") == 0)) ||
                                (((i < 48) && !((i % 8 == 0) || (i % 8 == 1)) && (String.Compare(gameTable.Table[i + 7].Item1.Substring(0, 3), "011") == 0 || String.Compare(gameTable.Table[i + 7].Item1.Substring(0, 3), "111") == 0) && String.Compare(gameTable.Table[i + 14].Item1.Substring(0, 3), "000") == 0))))
                            {
                                whichCanBeat[j] = i;
                                canBeat = true;
                                j++;
                            }
                        }
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        goto continueLabel;
                    }

                continueLabel:
                    if (!canBeat)
                    {
                        this.canBeat = false;
                        whichCanBeat[j] = -1;
                        Array.Copy(whichCanBeat, whichCanBeatLoc, 64);
                    }
                    else
                    {
                        whichCanBeat[j] = -1;
                        this.canBeat = true;
                        Array.Copy(whichCanBeat, whichCanBeatLoc, 64);
                    }
                }

                public void whichCanMove(out Int32[] whichCanMoveLoc)
                {
                    Int32 j = 0;
                    Boolean canMoveLoc = false;
                    var whichCanMove = new Int32[64];
                    whichCanMoveLoc = new Int32[64];
                    try
                    {
                        for (int i = 0; i < 64; i++)
                        {
                            var tuple = gameTable.Table[i];

                            //If the puppet is a null item move onwards
                            if (String.Compare(tuple.Item1.Substring(0, 3), "000") == 0)
                                continue;
                            //If there is a simple move available for a simple puppet
                            else if ((String.Compare(tuple.Item1.Substring(0, 3), "001") == 0) &&
                                (((i > 8) && (i % 8 != 0) && String.Compare(gameTable.Table[i - 9].Item1.Substring(0, 3), "000") == 0) ||
                                ((i > 7) && (i % 8 != 7) && String.Compare(gameTable.Table[i - 7].Item1.Substring(0, 3), "000") == 0)))
                            {
                                whichCanMove[j] = i;
                                canMoveLoc = true;
                                j++;
                            }
                            //If there is a simple move available for a king
                            else if ((String.Compare(tuple.Item1.Substring(0, 3), "101") == 0) &&
                                (((i > 8) && (i % 8 != 0) && String.Compare(gameTable.Table[i - 9].Item1.Substring(0, 3), "000") == 0) ||
                                ((i > 7) && (i % 8 != 7) && String.Compare(gameTable.Table[i - 7].Item1.Substring(0, 3), "000") == 0) ||
                                ((i < 55) && (i % 8 != 7) && String.Compare(gameTable.Table[i + 9].Item1.Substring(0, 3), "000") == 0) ||
                                ((i < 56) && (i % 8 != 0) && String.Compare(gameTable.Table[i + 7].Item1.Substring(0, 3), "000") == 0)))
                            {
                                whichCanMove[j] = i;
                                canMoveLoc = true;
                                j++;
                            }
                            //If the puppet is not a king and there is a beatable enemy against it 
                            else if ((String.Compare(tuple.Item1.Substring(0, 3), "001") == 0) &&
                                (((i > 18) && !((i % 8 == 0) || (i % 8 == 1)) && (String.Compare(gameTable.Table[i - 9].Item1.Substring(0, 3), "011") == 0 || String.Compare(gameTable.Table[i + 9].Item1.Substring(0, 3), "111") == 0) && (String.Compare(gameTable.Table[i - 18].Item1.Substring(0, 3), "000") == 0)) ||
                                ((i > 15) && !((i % 8 == 6) || (i % 8 == 7)) && (String.Compare(gameTable.Table[i - 7].Item1.Substring(0, 3), "011") == 0 || String.Compare(gameTable.Table[i - 7].Item1.Substring(0, 3), "111") == 0) && (String.Compare(gameTable.Table[i - 14].Item1.Substring(0, 3), "000") == 0))))
                            {
                                whichCanMove[j] = i;
                                canMoveLoc = true;
                                j++;
                            }
                            //If the puppet is a king and there is a beatable enemy against it
                            else if ((String.Compare(tuple.Item1.Substring(0, 3), "101") == 0) &&
                                (((i > 17) && !((i % 8 == 0) || (i % 8 == 1)) && (String.Compare(gameTable.Table[i - 9].Item1.Substring(0, 3), "011") == 0 || String.Compare(gameTable.Table[i - 9].Item1.Substring(0, 3), "111") == 0) && String.Compare(gameTable.Table[i - 18].Item1.Substring(0, 3), "000") == 0) ||
                                ((i > 15) && !((i % 8 == 6) || (i % 8 == 7)) && (String.Compare(gameTable.Table[i - 7].Item1.Substring(0, 3), "011") == 0 || String.Compare(gameTable.Table[i - 7].Item1.Substring(0, 3), "111") == 0) && String.Compare(gameTable.Table[i - 14].Item1.Substring(0, 3), "000") == 0) ||
                                ((i < 46) && !((i % 8 == 6) || (i % 8 == 7)) && (String.Compare(gameTable.Table[i + 9].Item1.Substring(0, 3), "011") == 0 || String.Compare(gameTable.Table[i + 9].Item1.Substring(0, 3), "111") == 0) && String.Compare(gameTable.Table[i + 18].Item1.Substring(0, 3), "000") == 0) ||
                                ((i < 48) && !((i % 8 == 0) || (i % 8 == 1)) && (String.Compare(gameTable.Table[i + 7].Item1.Substring(0, 3), "011") == 0 || String.Compare(gameTable.Table[i + 7].Item1.Substring(0, 3), "111") == 0) && String.Compare(gameTable.Table[i + 14].Item1.Substring(0, 3), "000") == 0)))
                            {
                                whichCanMove[j] = i;
                                canMoveLoc = true;
                                j++;
                            }
                        }
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        goto continueLabel;
                    }

                continueLabel:
                    if (!canMoveLoc)
                    {
                        this.canMove = false;
                        whichCanMoveLoc[j] = -1;
                        gameTable.gamePlayer.hasWon();
                    }
                    else
                    {
                        Array.Copy(whichCanMove, whichCanMoveLoc, 64);
                        whichCanMoveLoc[j] = -1;
                        this.canMove = true;
                    }
                }

                public Int16 whereToMove(Int16 fromNumInt, Boolean isABeat)
                {
                    Random rnd = new Random();

                    if (isABeat)
                    {
                    label0:
                        int randomNumber = rnd.Next(0, 4);
                        //If it's a simple puppet than only forward moves are available 
                        if (String.Compare(gameTable.Table[fromNumInt].Item1.Substring(0, 3), "001") == 0)
                            randomNumber = randomNumber % 2;

                        switch (randomNumber)
                        {
                            case 0:
                                if ((64 > (fromNumInt - 18)) && !(fromNumInt % 8 == 0 || fromNumInt % 8 == 1) && String.Compare(gameTable.Table[fromNumInt - 18].Item1, "00000") == 0)
                                    return (Int16)(fromNumInt - 18);
                                else goto label0;
                            case 1:
                                if ((64 > (fromNumInt - 14)) && !(fromNumInt % 8 == 6 || fromNumInt % 8 == 7) && String.Compare(gameTable.Table[fromNumInt - 14].Item1, "00000") == 0)
                                    return (Int16)(fromNumInt - 14);
                                else goto label0;
                            case 2:
                                if ((0 < (fromNumInt + 18)) && !(fromNumInt % 8 == 6 || fromNumInt % 8 == 7) && String.Compare(gameTable.Table[fromNumInt + 18].Item1, "00000") == 0)
                                    return (Int16)(fromNumInt + 18);
                                else goto label0;
                            case 3:
                                if ((0 < (fromNumInt + 14)) && !(fromNumInt % 8 == 0 || fromNumInt % 8 == 1) && String.Compare(gameTable.Table[fromNumInt + 14].Item1, "00000") == 0)
                                    return (Int16)(fromNumInt + 14);
                                else goto label0;
                        }
                    }
                    else
                    {
                    label1:
                        int randomNumber = rnd.Next(0, 4);
                        //If it's a simple puppet than only forward moves are available 
                        if (String.Compare(gameTable.Table[fromNumInt].Item1.Substring(0, 3), "001") == 0)
                            randomNumber = randomNumber % 2;

                        switch (randomNumber)
                        {
                            case 0:
                                if ((0 < (fromNumInt - 9)) && (fromNumInt % 8 != 0) && String.Compare(gameTable.Table[fromNumInt - 9].Item1, "00000") == 0)
                                    return (Int16)(fromNumInt - 9);
                                else goto label1;
                            case 1:
                                if ((0 < (fromNumInt - 7)) && (fromNumInt % 8 != 7) && String.Compare(gameTable.Table[fromNumInt - 7].Item1, "00000") == 0)
                                    return (Int16)(fromNumInt - 7);
                                else goto label1;
                            case 2:
                                if ((64 > (fromNumInt + 9)) && (fromNumInt % 8 != 7) && String.Compare(gameTable.Table[fromNumInt + 9].Item1, "00000") == 0)
                                    return (Int16)(fromNumInt + 9);
                                else goto label1;
                            case 3:
                                if ((64 > (fromNumInt + 7)) && (fromNumInt % 8 != 0) && String.Compare(gameTable.Table[fromNumInt + 7].Item1, "00000") == 0)
                                    return (Int16)(fromNumInt + 7);
                                else goto label1;
                        }
                    }
                    //By default, never used
                    return -1;
                }

                public override void changeToBeAKing(Int16 place)
                {
                    var tuple = gameTable.Table[place];
                    gameTable.Table[place] = new Tuple<string, PictureBox>("101" + tuple.Item1.Substring(3, 2), tuple.Item2);
                    tuple.Item2.Image = Properties.Resources.checkers_white_king;
                }

                //We should represent the table.Table in an Array, and thus move the puppets on the board - the player is with the white - property move will be NULL
                public override void movePuppet()
                {
                    //Generate random move for the computer
                    Int16 fromNumInt;
                    Int16 toNumInt;

                    if (!hasWon())
                    {
                        try
                        {
                            //If we want to move a puppet that does not exists in the current place.
                            whichCanMove(out this.whichPuppetCanMove);
                            playerCanBeat(out this.whichCanBeat);
                            logger.Write("White: ");

                            string canMoveConcat = "";
                            logger.Write("Which puppet can move");
                            for (int i = 0; whichPuppetCanMove[i] != -1; i++)
                            {
                                canMoveConcat += whichPuppetCanMove[i].ToString() + " ";
                            }
                            logger.Write(canMoveConcat);

                            string canBeatConcat = "";
                            logger.Write("Which can beat");
                            for (int i = 0; whichCanBeat[i] != -1; i++)
                            {
                                canBeatConcat += whichCanBeat[i].ToString() + " ";
                            }
                            logger.Write(canBeatConcat);

                            Random rand = new Random();
                            var tuple = new Tuple<string, PictureBox>("00000", null);

                        randomAgain:
                            if (this.canBeat)
                            {
                                int canBeatLength = 0;
                                while (whichCanBeat[canBeatLength] != -1)
                                    canBeatLength++;
                                int randomIndex = rand.Next(0, canBeatLength);
                                fromNumInt = (Int16)whichCanBeat[randomIndex];
                                if (String.Compare(gameTable.Table[fromNumInt].Item1, "00000") != 0)
                                    tuple = gameTable.Table[fromNumInt];
                                toNumInt = whereToMove(fromNumInt, true);
                            }
                            else
                            {
                                int canMoveLength = 0;
                                while (whichPuppetCanMove[canMoveLength] != -1)
                                    canMoveLength++;
                                int randomIndex = rand.Next(0, canMoveLength);
                                fromNumInt = (Int16)whichPuppetCanMove[randomIndex];
                                if (String.Compare(gameTable.Table[fromNumInt].Item1, "00000") != 0)
                                    tuple = gameTable.Table[fromNumInt];
                                else
                                    goto randomAgain;
                                toNumInt = whereToMove(fromNumInt, false);
                            }

                            gameTable.Table[fromNumInt] = gameTable.Table[toNumInt];
                            gameTable.Table[toNumInt] = tuple;
                            tuple = gameTable.Table[toNumInt];

                            //If it's a beat we should remove the interceeding puppet
                            if (fromNumInt - toNumInt == 18)
                            {
                                gameTable.Table[toNumInt + 9].Item2.Hide();
                                gameTable.Table[toNumInt + 9] = new Tuple<string, PictureBox>("00000", null);
                            }
                            else if (fromNumInt - toNumInt == 14)
                            {
                                gameTable.Table[toNumInt + 7].Item2.Hide();
                                gameTable.Table[toNumInt + 7] = new Tuple<string, PictureBox>("00000", null);
                            }

                            //if the puppet is on the last row at his opponent, then change it to be a king
                            if (String.Compare(tuple.Item1.Substring(0, 3), "001") == 0 && toNumInt < 8)
                            {
                                changeToBeAKing(toNumInt);
                                logger.Write(toNumInt + " puppet is now a king.");
                                gameForm.previous_step_listBox.Items.Add(toNumInt + " puppet is now a king.");
                            }

                                gameTable.renderTable();

                            hasWon();

                            var move = convertIntValuesToMove(fromNumInt, toNumInt);
                            gameTable.Moves.Add(new Tuple<string, string, string>("white", move.Key, move.Value));
                            gameForm.previous_step_listBox.Items.Add(gameTable.Moves[gameTable.NoOfMoves].Item1 + " - " + gameTable.Moves[gameTable.NoOfMoves].Item2 + ":" + gameTable.Moves[gameTable.NoOfMoves].Item3);
                            if (canBeat)
                                gameForm.previous_step_listBox.Items.Add("BEAT");
                            gameTable.NoOfMoves++;

                            logger.Write("white - " + move.Key + ":" + move.Value + " -> " + fromNumInt + ":" + toNumInt + "\n");

                        }

                        catch (Exception e)
                        {
                            logger.Write(e.ToString());
                            logger.Write("MoveStatus: false \n");
                            gameForm.error_label.Text = e.ToString();
                            gameForm.moveStatus = false;
                        }
                    }
                }
            }
        }

        private void help_button_Click(object sender, EventArgs e)
        {
            this.error_label.Text = "The move that you should give in must be in the form: 'from':'to'";
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            if (!won)
            {
                var childForm = new AbortationForm(this);
                childForm.ShowDialog();
            }
            else
                this.Close();
        }
    }
}
 
