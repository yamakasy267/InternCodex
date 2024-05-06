class InternCodex
{
    static void Main(string[] args)
    {
        string path = Console.ReadLine();
        if (path == "" ) // проверка, что путь был указан
        {
            Console.WriteLine("Путь не указан");
            return;
        }
        
        Analizator analis = new Analizator(path);
        //analis.generator(5); //функция генерации рандомных исходныйх данных. параметром принимает в себя количество файлов кот. нужно сгенерировать
        analis.Search();
    }
}

class Analizator
{
    private string FilePath;
    public Analizator(string filePath)
    {
        this.FilePath = filePath;
    }
    public void Search()
    {
        SortedDictionary<int, int> listNumbers = new SortedDictionary<int, int>();
        Dictionary<int, int> blackList = new Dictionary<int, int>();
        int numb;
        string[] dirs = Directory.GetFiles(this.FilePath, "*.txt"); // получение списка файлов в каталоге
        dirs = dirs.Where(val => val != $"{this.FilePath}\\result.txt").ToArray(); // убираем результирующий файл, что бы можно было несколько раз запускать программу и не удалять этот файл


        //Логика данного алгоритма такова: бежим по масиву со списком файлов, и открываем каждый файл. Из файла считываем числа
        //проверяем число на деление с остатком, если подходит то проверяем есть ли оно уже в нашем списке. Если оно есть
        //удаляем его и добавляем в черный список. Если его нет, то проверяем черный список, если и там нет, то добавляем в список.
        //Если же есть в серном списке, просто пропускаем.
        foreach (string dir in dirs)
        {
            using (StreamReader fstream = new StreamReader(dir))
            {
                while (!fstream.EndOfStream)
                {
                    //Тут происходит проверка, на то что , то что мы считали является числом
                    try
                    {
                        numb = int.Parse(fstream.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Это было не число");
                        continue;
                    }

                    if (numb % 4 == 3)
                    {
                        if (listNumbers.ContainsKey(numb))
                        {
                            listNumbers.Remove(numb);
                            blackList.Add(numb, 0);
                        }
                        else if (blackList.ContainsKey(numb))
                        {
                            continue;
                        }
                        else
                        {
                            listNumbers.Add(numb, 0);
                        }
                    }
                }
            }

        }

        //запись результата в файл
        using (StreamWriter wstream = new StreamWriter($"{this.FilePath}/result.txt")) 
        {
            foreach (var iterator in listNumbers.Reverse())
            {
                wstream.WriteLine(iterator.Key);
            }
        }
            
    }
    public void generator(int count_file)
    {
        Random rnd = new Random();
        for (int i =0; i < count_file; ++i) 
        {
            int count = rnd.Next(100, 1000);
            using (StreamWriter fstream = new StreamWriter($"{this.FilePath}/Numbers{i}.txt", false))
            {
                for (int j = 0; j < count; j++)
                {
                    fstream.WriteLine(rnd.Next());
                }

            }
        }
        
    }
}