using System.Reflection;
class Program
{
    static void Main(string[] args)

    {
        //var assemblyPath = @"C:\Users\bastis\Documents\Galia_Reflection.dll";

        var assembly = Assembly.LoadFrom("Galia_Reflection");

        var password = "";


        Console.WriteLine("Информация о сборке: {0}", assembly.FullName);
        Console.WriteLine();
        Console.WriteLine("Сборка имеет следующие типы:");
        Console.WriteLine();
        foreach (var type in assembly.GetTypes())
        {

            Console.WriteLine("Название типа: {0}", type.Name);

            if (type.Name == "Secret")
            {
                foreach (var i in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static))
                {
                    Console.WriteLine("Класс Secret имеет поле {0} со значением {1}", i.Name, i.GetValue(null));
                    if (i.Name == "password")
                    {
                        password = (string)i.GetValue(null);
                    }
                }
            }
            Console.WriteLine();

            Console.WriteLine("Методы типа:");   
            foreach (var i in type.GetMethods())
            {
                Console.WriteLine(i.Name);
            }
            Console.WriteLine();

            if (type.GetProperties() != null)
            {
                foreach (var i in type.GetProperties())
                {
                    Console.WriteLine("Свойство типа: {0}", i.Name);
                }
            }         
            Console.WriteLine();

            /*foreach (var member in type.GetMembers())
            {
                Console.WriteLine($"Информация о члене: {member.DeclaringType} {member.MemberType} {member.Name}");
            }
            Console.WriteLine();*/

            foreach (var method in 
                type.GetMethods(BindingFlags.Static | BindingFlags.Public).Cast<MethodInfo>().Concat(
                type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Cast<MethodInfo>()))
            {
                Console.WriteLine("Подробно о методе:");
                Console.WriteLine($"Название метода: {method.Name}, возвращаемый тип: {method.ReturnType}");

                var parameters = method.GetParameters();
                for (int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];
                    string modificator = "";
                    if (param.IsIn)
                    {
                        modificator = "in";
                    }
                    else if (param.IsOut)
                    {
                        modificator = "out";
                    }
                    Console.Write($"Принимаемый параметр: {param.ParameterType.Name}{modificator} {param.Name}");
                    if (param.HasDefaultValue)
                    {
                        Console.Write($"={param.DefaultValue}");
                    }
                    if (i < parameters.Length - 1)
                    {
                        Console.Write(", ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine("Вызовы методов!!!");
        Console.WriteLine();

        var testType = assembly.GetType("Galia_Reflection.Test");
        var instanceTest = Activator.CreateInstance(testType);
        var testMethod = testType.GetMethod("Run");

        Console.WriteLine("Вызываю метод Run");
        Console.WriteLine();

        testMethod.Invoke(instanceTest, null);

        Console.WriteLine();
        Console.WriteLine("Вызов завершен");
        Console.WriteLine();


        var secretType = assembly.GetType("Galia_Reflection.Secret");
        var secretMethod = secretType.GetMethod("ShowMeYourSecret");
        
        Console.WriteLine();
        Console.WriteLine("Вызываю метод ShowMeYourSecret");
        Console.WriteLine();

        var result = secretMethod?.Invoke(null, parameters: new object[] { password });

        Console.WriteLine(result);
        Console.WriteLine();
        Console.WriteLine("Вызов завершен");
        Console.WriteLine();
    }
}
