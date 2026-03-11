using System;

namespace galo2
{
    internal class Program

    {
        static void Main(string[] args)
        {
            StartGame();
        }


        static void StartGame()
        {
            Console.WriteLine("Bem-vindo ao jogo do galo!");
            Console.WriteLine("Escolha o modo de jogo:");
            Console.WriteLine("1. Jogador Vs Computador");
            Console.WriteLine("2. Jogador Vs Jogador");
            Console.WriteLine("3. Sair");

            int mode = int.Parse(Console.ReadLine());

            string[,] board = { };

            switch (mode)
            {
                case 1:
                    Console.WriteLine("Modo Jogador Vs Computador selecionado.");
                    Console.WriteLine("Jogador, insira o seu nickname:");
                    string playerNickname = Console.ReadLine();

                    Console.WriteLine("Bem-vindo, " + playerNickname + "!");
                    board = GetBoardSize();
                    StartPlay(playerNickname, null, board);
                    break;

                case 2:
                    Console.WriteLine("Modo Jogador Vs Jogador selecionado.");
                    Console.WriteLine("Jogador 1, insira o seu nickname:");
                    string player1Nickname = Console.ReadLine();
                    Console.WriteLine("Jogador 2, insira o seu nickname:");
                    string player2Nickname = Console.ReadLine();

                    Console.WriteLine("Bem-vindos, " + player1Nickname + " e " + player2Nickname + "!");
                    board = GetBoardSize();
                    StartPlay(player1Nickname, player2Nickname, board);
                    break;

                case 3:
                    Console.WriteLine("Saindo do jogo. Até à próxima!");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Opção inválida. Por favor, escolha uma opção válida.");
                    return;
            }
        }

        static void PrintBoard(string[,] board)
        {
            int n = board.GetLength(0);
            Console.WriteLine();

            for (int i = 0; i < n; i++)
            {
                // separator top line
                for (int k = 0; k < n; k++)
                    Console.Write("+---");
                Console.WriteLine("+");

                // board body and values
                for (int j = 0; j < n; j++)
                {
                    string cell = board[i, j];
                    if (String.IsNullOrEmpty(cell))
                        cell = " ";

                    Console.Write("| " + cell + " ");
                }
                Console.WriteLine("|");
            }

            // Separator bottom line
            for (int k = 0; k < n; k++)
                Console.Write("+---");
            Console.WriteLine("+");
            Console.WriteLine();
        }

        static bool MakeMove(string[,] board, int row, int col, string player)
        {
            int n = board.GetLength(0);

            // validate if move is within board limits
            if (row < 0 || row >= n || col < 0 || col >= n)
            {
                Console.WriteLine("Jogada fora do tabuleiro! Tente novamente.");
                return false;
            }

            // validate if cell is already occupied
            if (!string.IsNullOrEmpty(board[row, col]) && board[row, col] != " ")
            {
                Console.WriteLine("Essa posição já está ocupada! Tente novamente.");
                return false;
            }

            Console.Clear();
            //update board with move
            board[row, col] = player;
            return true;
        }

        static string[,] GetBoardSize()
        {
            Console.Clear();
            Console.WriteLine("Selecione um tamanho de tabuleiro:");
            Console.WriteLine("1. 3x3");
            Console.WriteLine("2. 5x5");
            Console.WriteLine("3. 7x7");

            int option = int.Parse(Console.ReadLine());
            int boardSize;

            switch (option)
            {
                // choose board size
                case 1: boardSize = 3; break;
                case 2: boardSize = 5; break;
                case 3: boardSize = 7; break;
                default:
                    Console.WriteLine("Opção inválida, será usado 3x3.");
                    boardSize = 3;
                    break;
            }

            string[,] board = new string[boardSize, boardSize];

            // print board with size based on input (boardSize)
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    board[i, j] = " ";

            Console.Clear();
            return board;
        }

        static void StartPlay(string player1Nickname, string player2Nickname, string[,] board)
        {
            Console.WriteLine("Tabuleiro " + board.GetLength(0) + "x" + board.GetLength(1) + " selecionado.");
            char player1 = 'X', player2 = 'O';
            bool isPlayer1Turn = true;
            int moves = 0;
            int maxMoves = board.GetLength(0) * board.GetLength(1);

            while (true)
            {
                PrintBoard(board);

                string currentPlayerName = isPlayer1Turn ? player1Nickname :
                                           (player2Nickname ?? "Computador");
                char currentSymbol = isPlayer1Turn ? player1 : player2;

                if (isPlayer1Turn || player2Nickname != null)
                {
                    // Player move
                    bool validMove = false;
                    while (!validMove)
                    {
                        Console.WriteLine(currentPlayerName + ", é a sua vez! Insira a linha e a coluna (ex: 0 0):");
                        string[] input = Console.ReadLine().Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);

                        if (input.Length != 2 ||
                            !int.TryParse(input[0], out int row) ||
                            !int.TryParse(input[1], out int col))
                        {
                            Console.WriteLine("Entrada inválida. Use linha coluna (ex: 0 0).");
                            continue;
                        }

                        validMove = MakeMove(board, row, col, currentSymbol.ToString());
                    }
                }
                else
                {
                    // PC move
                    Console.WriteLine("Jogada do Computador...");
                    Random rand = new Random();
                    int row, col;
                    do
                    {
                        row = rand.Next(board.GetLength(0));
                        col = rand.Next(board.GetLength(1));
                    } while (!string.IsNullOrEmpty(board[row, col]) && board[row, col] != " ");

                    board[row, col] = currentSymbol.ToString();
                }

                moves++;

                // check victory
                if (CheckWinner(board, currentSymbol.ToString()))
                {
                    PrintBoard(board);
                    Console.WriteLine("Jogador " + currentPlayerName + " (" + currentSymbol + ") venceu!");
                    EndGame();


                }

                // check draw
                if (moves == maxMoves)
                {
                    PrintBoard(board);
                    Console.WriteLine("Empate! Não há mais jogadas possíveis.");
                    EndGame();
                }

                isPlayer1Turn = !isPlayer1Turn;
            }
        }

        static void EndGame()
        {
            Console.WriteLine("Deseja jogar novamente? (s/n)");
            string input = Console.ReadLine().ToLower();

            // validate input for replay
            while (input != "s" && input != "n")
            {
                Console.WriteLine("Entrada inválida. Por favor, digite 's' para sim ou 'n' para não.");
                input = Console.ReadLine().ToLower();
            }

            if (input == "s")
            {
                Console.Clear();
                StartGame();
            }
            else
            {
                Console.WriteLine("Obrigado por jogar! Até à próxima!");
                Environment.Exit(0);
            }
        }

        static bool CheckWinner(string[,] board, string player)
        {
            int n = board.GetLength(0);

            // lines
            for (int i = 0; i < n; i++)
            {
                bool win = true;
                for (int j = 0; j < n; j++)
                {
                    if (board[i, j] != player)
                    {
                        win = false;
                        break;
                    }
                }
                if (win) return true;
            }

            // columns
            for (int j = 0; j < n; j++)
            {
                bool win = true;
                for (int i = 0; i < n; i++)
                {
                    if (board[i, j] != player)
                    {
                        win = false;
                        break;
                    }
                }
                if (win) return true;
            }

            // diagonal
            bool diag1 = true;
            for (int i = 0; i < n; i++)
            {
                if (board[i, i] != player)
                {
                    diag1 = false;
                    break;
                }
            }

            if (diag1) return true;

            // check for diagonal (top-left to bottom-right)
            bool diag2 = true;
            for (int i = 0; i < n; i++)
            {

                // check for anti-diagonal (top-right to bottom-left)
                if (board[i, n - 1 - i] != player)
                {
                    diag2 = false;
                    break;
                }
            }
            if (diag2) return true;

            return false;
        }
    }
}
