namespace QuizMaster_Challenge
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("You have 2 questions, each question has 15 sec \n");
                StartQuiz();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Quiz Completed. Thank you for participating!");
            }
        }

        static void StartQuiz()
        {
            var questionsAndAnswers = new List<(string Question, string[] Options, string Answer)>
            {
                ("Q1 - What is the capital of Jordan?", new[] {"a) Amman", "b) Irbid", "c) Ajloun"}, "a"),
                ("Q2 - What is 2 + 2?", new[] {"a) 3", "b) 4", "c) 8"}, "b")
            };

            int score = 0;
            int timeLimit = 15; // 15 seconds for each question

            foreach (var (question, options, answer) in questionsAndAnswers)
            {
                string userAnswer = null;
                bool validAnswer = false;

                Console.WriteLine(question);
                foreach (var option in options)
                {
                    Console.WriteLine(option);
                }

                userAnswer = GetUserAnswerWithTimer(timeLimit);

                if (userAnswer == null || (userAnswer.ToLower() != "a" && userAnswer.ToLower() != "b" && userAnswer.ToLower() != "c"))
                {
                    Console.WriteLine("Invalid input! Please answer with 'a', 'b', or 'c'.\n");
                    continue; // Skip further processing for this question
                }

                if (userAnswer.Trim().Equals(answer, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Correct!");
                    score++;
                }
                else
                {
                    Console.WriteLine($"Incorrect. The correct answer is: {answer}");
                }
            }

            Console.WriteLine($"\nYour final score is: {score}/{questionsAndAnswers.Count}");
        }

        static string GetUserAnswerWithTimer(int timeLimit)
        {
            string userAnswer = null;
            var cts = new CancellationTokenSource();

            var task = Task.Run(() =>
            {
                int timeLeft = timeLimit;
                while (!cts.Token.IsCancellationRequested && timeLeft > 0)
                {
                    if (Console.KeyAvailable)
                    {
                        userAnswer = Console.ReadLine();
                        cts.Cancel();
                    }
                    else
                    {
                        Console.Write($"\rTimer: {timeLeft} sec");
                        Thread.Sleep(1000);
                        timeLeft--;
                    }
                }
                return userAnswer;
            }, cts.Token);

            if (!task.Wait(TimeSpan.FromSeconds(timeLimit)))
            {
                Console.WriteLine("\nTime's up! Moving to the next question.\n");
                cts.Cancel();
            }

            return userAnswer;
        }
    }
}
