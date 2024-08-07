using System.Timers;

class GameWords
{
    private const int TimerInterval = 15000;
    private const int MaxWordLength = 33;
    private const int MinWordLength = 8;
    private const int Zero = 0;
    private const string Space = " ";

    private List<string> _enteredWords = new List<string>();
    private int _playerNumber = 2;
    private string? _player1;
    private string? _player2;
    private string? _word;
    private string? _littleWord;
    private bool _loss = false;

    private static void Main(string[] args)
    {
        GameWords gameWords = new();        

        Print("Добро пожаловать в игру 'Слова'! Введите свои имена.\n");

        gameWords.SetPlayer(1, out gameWords._player1);
        gameWords.SetPlayer(2, out gameWords._player2);

        Print($"Первым ходит игрок {gameWords._player1}!\n");

        gameWords.SetWord();

        System.Timers.Timer timer = new(TimerInterval);
        timer.Elapsed += Timer_Elapsed;

        while (!gameWords._loss)
        {
            switch (gameWords._playerNumber)
            {
                case 1:
                    StartTimer(timer);

                    Print($"\n{gameWords._player1}, ваша очередь!");

                    gameWords.SetLittleWord();

                    if (!gameWords._loss) gameWords._playerNumber = 2;
                    break;

                case 2:
                    StartTimer(timer);

                    Print($"\n{gameWords._player2}, ваша очередь!");

                    gameWords.SetLittleWord();

                    if (!gameWords._loss) gameWords._playerNumber = 1;
                    break;
            }
        }        
        gameWords.PrintLoser();
    }


    /// <summary>
    /// Выводит проигравшего игрока.
    /// </summary>
    private void PrintLoser()
    {
       Print($"Игрок {_playerNumber} проиграл!");
    }

    /// <summary>
    /// Считывает имя игрока с помощью метода Read().
    /// Имя не может быть пустым или содержать пробелы.
    /// </summary>
    /// <returns>Имя игрока.</returns>
    private string GetPlayerName()
    {
        string name = Read();
        while ((name.Length == Zero) || (name.Contains(Space)))
        {
            Print($"Неверный ввод имени. Попробуйте ещё раз: ");
            name = Read();
        }
        return name;
    }

    /// <summary>
    /// Устанавливает имя игрока.
    /// </summary>
    /// <param name="playerNumber">Номер игрока.</param>
    /// <param name="playerName">Имя игрока.</param>
    private void SetPlayer(int playerNumber, out String playerName)
    {
        Print($"Игрок {playerNumber}: ");
        playerName = GetPlayerName();
    }

    /// <summary>
    /// Считывает главное слова для игры с помощью метода Read().
    /// Слово не может быть меньше минимального и больше максимального заданных значений.
    /// </summary>
    /// <returns>Главное слово для игры.</returns>
    private string GetWord()
    {
        string word = Read();
        bool isLetter = word.All(Char.IsLetter);
        while ((word.Length > MaxWordLength) || (word.Length < MinWordLength) || (!isLetter))
        {
            Print("Неверный ввод слова! Попробуйте снова: ");
            word = Read();
            isLetter = word.All(Char.IsLetter);
        }
        return word;
    }

    /// <summary>
    /// Устанавливает главное слово для игры.
    /// </summary>
    private void SetWord()
    {
        Print($"Введите слово от {MinWordLength} до {MaxWordLength} букв: ");
        _word = GetWord();
        Print($"Слово для вашей игры: {_word}");
    }

    /// <summary>
    /// Считывает слово из букв главного для игры слова с помощью метода Read() и запускает проверку этого слова.
    /// Проверяет, что слово не является пустым и не содержит пробелы, и вызывает метод CheckLittleWord().
    /// </summary>
    /// <returns>Слово из букв главного слова.</returns>
    private string GetLittleWord()
    {
        bool cheking = true;
        _loss = false;
        _littleWord = Read();

        while (cheking)
        {
            if ((_littleWord.Length) == Zero || (_littleWord.Contains(Space)))
            {
                _loss = true;
                cheking = false;
            }
            else
            {
                if (!CheckLittleWord())
                {
                    Print("Неверный ввод, попробуйте снова: ");
                    _littleWord = Read();
                }
                else cheking = false;
            }
        }
        return _littleWord;
    }

    /// <summary>
    /// Запускает три проверки слова из букв главного слова:
    /// 1) в слове только буквы из главного слова;
    /// 2) слово не было раньше введено;
    /// 3) количество определённых букв совпадает с количеством этих букв в главном слове.
    /// </summary>
    /// <returns>Общее значение трёх проверок типа bool.</returns>
    private bool CheckLittleWord()
    {
        bool letterCheck;
        bool letterAmountCheck;
        bool isInList;

        letterCheck = CheckLittleWordLetters();
        isInList = CheckEnteredWords();
        letterAmountCheck = CheckLettersAmount();

        return letterCheck && isInList && letterAmountCheck;
    }

    /// <summary>
    /// Проверяет, что слово состоит только из букв главного слова.
    /// </summary>
    /// <returns>Результат проверки типа bool.</returns>
    private bool CheckLittleWordLetters()
    {
        return _littleWord.ToLower().All(ch => _word.ToLower().Contains(ch));
    }

    /// <summary>
    /// Проверяет, что слово не было ранее введено.
    /// </summary>
    /// <returns>Результат проверки типа bool.</returns>
    private bool CheckEnteredWords()
    {
        return !_enteredWords.Contains(_littleWord.ToLower());
    }

    /// <summary>
    /// Проверяет, что количество определённых букв в слове из букв главного слова совпадает с количеством этих букв в главном слове.
    /// </summary>
    /// <returns>Результат проверки типа bool.</returns>
    private bool CheckLettersAmount()
    {
        bool letterAmount = false;
        var lettersInWord = _word.ToLower().GroupBy(ch => ch);
        var lettersInLittleWord = _littleWord.ToLower().GroupBy(ch => ch);

        foreach (var letter1 in lettersInLittleWord)
        {
            foreach (var letter2 in lettersInWord)
            {
                if (letter1.Count() > letter2.Count())
                    letterAmount = false;
                else letterAmount = true;
            }
        }
        return letterAmount;
    }

    /// <summary>
    /// Устанавливает значение слова из букв главного слова и добавляет его в список введённых слов.
    /// </summary>
    private void SetLittleWord()
    {
        Print($"У вас {TimerInterval/1000} секунд. Введите слово, состоящее из букв главного слова: ");
        _littleWord = GetLittleWord();
        _enteredWords.Add(_littleWord.ToLower());
    }

    /// <summary>
    /// Метод, вызываемый таймером. Останавливает таймер и прекращает работу программы.
    /// </summary>
    /// <param name="sender"> Таймер</param>
    /// <param name="e"></param>
    private static void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        ((System.Timers.Timer)sender).Stop();

        Print("Вы проиграли!");

        Environment.Exit(0);
    }

    /// <summary>
    /// Запускает таймер.
    /// </summary>
    /// <param name="timer"> Таймер.</param>
    private static void StartTimer(System.Timers.Timer timer)
    {
        timer.Stop();
        timer.Start();
    }

    /// <summary>
    /// Метод вывода текста на консоль.
    /// </summary>
    /// <param name="text">Выводимый текст.</param>
    private static void Print(String text)
    {
        Console.WriteLine(text);
    }

    /// <summary>
    /// Чтение текста из консоли.
    /// </summary>
    /// <returns>Текст из консоли.</returns>
    private String Read()
    {
        return Console.ReadLine();
    }
}