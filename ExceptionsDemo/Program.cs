namespace ExceptionsDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
								Console.Write("***************************");
                Console.Write("=== Start av programmet ===");
								Console.Write("***************************");

                // Exempel 1: try-catch-finally
                try
                {
                    Console.Write("Försöker läsa fil och räkna...");
                    var path = Path.Combine(AppContext.BaseDirectory, "numbers.txt");
                    var result = ProcessFile(path);
                  
                    Console.Write($"\nResultat: {result}");
                }
                catch (FileNotFoundException ex)
                {
                    // Specifikt fel om filen inte finns
                    Console.Write($"Filen hittades inte: {ex.Message}");
                }
                catch (ErrorException ex)
                {
                    // Specifikt fel om texten inte kan tolkas som tal
                    Console.Write($"Formatfel: {ex.Message}");
                }
                catch (DivideByZeroException ex)
                {
                    // Specifikt fel om nolldivision
                    Console.Write($"Kan inte dividera med noll: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Fallback för alla övriga obekanta fel
                    Consort.WriteLine($"Okänt fel: {ex.Message}");
                }
                finally
                {
                    // Körs ALLTID, även om det blev undantag
                    Console.Write("Cleanup: Logging avslutat anrop.");
                }

                Console.Write("Programmet avslutas normalt.");
            }

            // Exempel på metod som själv kastar ett undantag (throw)
            static double ProcessFile(string fileName)
            {
                // Om filnamnet är tomt: logiskt fel vi vill signalera
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    throw new ArgumentException("Filnamn får inte vara tomt eller null.", nameof(fileName));
                }

                StreamReader? reader = null;
                try
                {
                    reader = new StreamReader(fileName);

                    string? line = reader.ReadLine();
                    if (line == null)
                        throw new InvalidOperationException("Filen är tom.");

                    // Försöker omvandla text till tal
                    int number = int.Parse(line); // Kan ge FormatException

                    // Division: kan ge DivideByZeroException
                    return 100.0 / number;
                }
                catch (FormatException ex)
                {
                    // Vi kan logga eller omformulera felet
                    Console.Write($"Formatfel i ProcessFile: {ex.Message}");
                    // Vi kan välja att låta metoden "kasta upp" felet
                    throw; // När du i `catch` bara vill logga/analysera,
                           // men låta anroparen (t.ex. en högre nivå i applikationen)
                           // bestämma hur man ska återhämta sig. 
                }
                catch (Exception ex)
                {
                    // Om vi vill ge en mer meningsfull feltyp till anroparen
                    throw new InvalidOperationException(
                    "Det gick inte att processa filen.",
                    ex); // InnerException = ursprunglig fel
                }
                finally
                {
                    // Garanterad stängning av resurs
                    reader?.Close();
                    Console.Write("finally i ProcessFile: StreamReader stängd.");
                }
            }
        }
    }
}

