using System.Reflection;
using System.Text.RegularExpressions;


string dashes = "\n" + new String('-', 50) + "\n";

string text = "Now is the time for all good men to come to the aid of their country.";


Intro(); // header 2

// Create an instance of the CountTokensDelegateType type.
// 
// This delegate estimates the number of 'tokens' in a string.  
// There are many ways to do this, so moving the details into a delegate
// allows us to pass in the details to the object that needs it.
// This is not unlike passing a Comparison delegate into a Sort method.
//
Console.WriteLine(dashes);
Console.WriteLine("1. Let's create a delegate instance named 'countTokensDelegate' using a lambda function that merely calls Helpers.CountTokens(s).");
Console.WriteLine("Helpers.CountTokens takes a string and returns an int, so we use Func<string,int> as the declared type of the delegate instance.");
Console.WriteLine("We don't care about how countTokensDelegate does its work. We only care that it takes a string and returns an int.");

PrintCode("Func<string, int> countTokensDelegate = (string s) => { return Helpers.CountTokens(s); };");
Func<string, int> countTokensDelegate = (string s) => { return Helpers.CountTokens(s); };

Console.WriteLine("Let's use the just-created delegate instance 'countTokensDelegate' to count the tokens in our string:");
PrintCode("Console.WriteLine($\"The text '{text}' has an estimated {countTokensDelegate(text)} tokens.\");");
PrintOutput($"The text '{text}' has an estimated {countTokensDelegate(text)} tokens.");

/////////////

Console.WriteLine("\nNote that we can declare our own custom delegate type and use it in place of the more convenient Func<string, int>.");
Console.WriteLine("In the static Helpers class, let's declare a delegate type 'CountTokensDelegateType' that takes a string and returns an int.");
PrintCode("public delegate int CountTokensDelegateType(string s);");
Console.WriteLine("Let's use the custom delegate definition to create a delegate instance, countTokensDelegate2.");
PrintCode("Helpers.CountTokensDelegateType countTokensDelegate2 = (string s) => { return Helpers.CountTokens(s); });");
Helpers.CountTokensDelegateType countTokensDelegate2 = (string s) => { return Helpers.CountTokens(s); };
Console.WriteLine("Finally, let's use countTokensDelegate2 to count the tokens in our string:");
PrintCode("Console.WriteLine($\"The text '{text}' has an estimated {countTokensDelegate2(text)} tokens.\");");
PrintOutput($"The text '{text}' has an estimated {countTokensDelegate2(text)} tokens.");

/////////////// 
Console.WriteLine(dashes);
Console.WriteLine("2. Let's create the countTokensDelegate instance by assigning the Helpers.CountTokens method directly to it.");
Console.WriteLine("We can do this because the signature of Helpers.CountTokens matches the signature of the countTokensDelegate instance.");
PrintCode("countTokensDelegate = Helpers.CountTokens;");
countTokensDelegate = Helpers.CountTokens;                                                   // We can also just assign the local method name to the delegate.
Console.WriteLine("Let's use use the 'new' countTokensDelegate. It works the same.");
PrintCode($"PrintOutput($\"The text '{{text}}' has an estimated {{countTokensDelegate(text)}} tokens.\")");
PrintOutput($"The text '{text}' has an estimated {countTokensDelegate(text)} tokens.");

// Use reflection to inspect the delegate.
//
//
///////////////////////////////////////////////////////////////////////////////////////
Console.WriteLine(dashes);
Console.WriteLine("3. Delegate instances are just methods, so let's use .NET's Reflection GetMethodInfo() to inspect the countTokensDelegate delegate instance.");
PrintCode("MethodInfo minfo = countTokensDelegate.GetMethodInfo();\nParameterInfo[] parameters = minfo.GetParameters();\nType returnType = minfo.ReturnType;");
MethodInfo minfo = countTokensDelegate.GetMethodInfo();
ParameterInfo[] parameters = minfo.GetParameters();
PrintOutput("Parameters : ");
Array.ForEach<ParameterInfo>(parameters, (parameter) => PrintOutput($"{parameter.Name} {parameter.ParameterType}"));
Type returnType = minfo.ReturnType;
PrintOutput($"Return Type (minfo.ReturnType): {returnType.Name} {returnType.FullName}");

///////////////////////////////////////////////////////////////////////////////////////
Console.WriteLine(dashes);
Console.WriteLine("4. The .NET framework makes heavy use of delegates.\nConsider the Array.Sort<T>() method. The method takes two arguments:\n- an array of T (in our case T will be 'string') \n- A Func<string,string,int> delegate instance that mimicks the Comparison<T> delegate by indicating the correct order of the two arguments.");
Console.WriteLine("\n\nWe'll use a lambda function to satisfy the 'Func<string,string,int>' signature of the delegate argument expected by Array.Sort<string>.");

PrintCode("Array.Sort<string>(words, (string a, string b) =>\n            {\n                return a.CompareTo(b);\n            });");

// Delegates are used frequently by the .NET framework. 
// Here we use a lambda expression to satisfy the contract of the Comparison<T> 
// generic delegate used by one overload of Array.Sort() 
//
string[] words = text.Split(" ");                     // build the words array
Array.Sort<string>(words, (string a, string b) =>
{
    return a.CompareTo(b);
});

PrintCode("OutputWords(words);");
OutputWords(words);

///////////////////////////////////////////////////////////////////////////////////////
Console.WriteLine(dashes);
Console.WriteLine("5. Let's call Array.Sort<T> again, but this time instead of using an inline delegate, we'll create an explicitly-declared Comparison<string> delegate, which has a Func<string,string,int> signature.");

// We can do the same thing using a Comparison<T> delegate.
// T is the type of the two objects being compared. 
//
words = text.Split(" ");

PrintCode("Comparison<string> comparison = (a, b) => a.CompareTo(b);");
Comparison<string> comparison = (a, b) => a.CompareTo(b);  // create Comparison delegate instance.

Console.WriteLine("Let's call Sort with the comparison delegate:");
PrintCode("Array.Sort(words, comparison);");
Array.Sort(words, comparison);
PrintCode("OutputWords(words);");
OutputWords(words);

///////////////////////////////////////////////////////////////////////////////////////
// Use a local function (DoComparison) as the comparison delegate.
//
//
Console.WriteLine(dashes);
Console.WriteLine("6. Let's call Array.Sort again, but this time we'll assign a local method name to a declared Comparison<string> generic delegate instance.");
Console.WriteLine("It goes without saying that the DoComparison method conforms to the Comparison<string> signature -- it accepts two strings and returns an int.)");

PrintCode("Comparison<string> comp = DoComparison;");

Console.WriteLine("Here's DoComparison:");
PrintCode("static int DoComparison(string a, string b)\r\n{\r\n    return a.CompareTo(b);\r\n}");

words = text.Split(" ");
Comparison<string> comp = DoComparison;
Console.WriteLine("Let's use the 'comp' delegate to sort the strings.");
PrintCode("Array.Sort<string>(words, comp);");
Array.Sort<string>(words, comp);
PrintCode("OutputWords(words);");
OutputWords(words);

///////////////////////////////////////////////////////////////////////////////////////

Console.WriteLine(dashes);
Console.WriteLine("7. Now let's use a delegate to change the default sort order of a custom object.");
Console.WriteLine("The class Employee contains only the LastName and ZipCode. Its default sort order is LastName: ");
PrintCode("public class Employee : IComparable<Employee>\r\n    {\r\n        public string LastName { get; set; } = \"\";\r\n        public string ZipCode { get; set; } = \"\";\r\n\r\n        public int CompareTo(Employee? other)\r\n        {\r\n            return LastName.CompareTo(other?.LastName);\r\n        }\r\n    }");
Console.WriteLine("\n");

// Sort Employees in non-standard way by using a delegate.
// This is mentioned in the article.  
//
// Declare the Employee list and populate it. (See the bottom of the file for class Employee)
//
List<Employee> emps = new List<Employee>()
            {
                new Employee { LastName = "Smith", ZipCode = "22345" },
                new Employee { LastName = "Johnson", ZipCode = "47890" },
                new Employee { LastName = "Williams", ZipCode = "13456" },
                new Employee { LastName = "Jones", ZipCode = "38901" },
                new Employee { LastName = "Brown", ZipCode = "54567" }
            };

// Create a simple Action delegate to print the employee list 
//
Console.WriteLine("Just for fun, let's create a delegate to print the employee list after we're done processing it.");
Console.WriteLine("This is an Action delegate that takes a single List<Employee> argument and outputs the employees to the console.");
PrintCode("Action<List<Employee>> OutputEmps = (List<Employee> emps) =>\r\n            {\r\n                foreach (Employee emp in emps) { PrintOutput(emp.LastName + \" \" + emp.ZipCode); }\r\n            };\r\n");

Action<List<Employee>> OutputEmps = (List<Employee> emps) =>
{
    //foreach (Employee emp in emps) { Console.WriteLine(emp.LastName + " " + emp.ZipCode); }
    emps.ForEach((emp) => PrintOutput(emp.LastName + " " + emp.ZipCode));
};

// sort in "normal" order, which for the Employee class is LastName
Console.WriteLine(dashes);
Console.WriteLine("8. Let's sort employees by LastName, the natural order defined in the Employee class.");
PrintCode("emps.Sort();\nOutputEmps(emps);");
emps.Sort();
OutputEmps(emps);

// zip code order using delegate (anonymous lambda)
//
Console.WriteLine(dashes);
Console.WriteLine("9. Let's override the default Employee sort order by passing a lambda expression to the Sort method. We just use a lambda expression.");
PrintCode("emps.Sort((e1, e2) => e1.ZipCode.CompareTo(e2.ZipCode));\nOutputEmps(emps);");
emps.Sort((e1, e2) => e1.ZipCode.CompareTo(e2.ZipCode));
OutputEmps(emps);


Console.WriteLine("\n\nEND");

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Helper methods 
//

// Intro: Displays the header.
//
static void Intro()
{
    Console.ResetColor();
    Console.WriteLine("ACTIONs and FUNCs");
    Console.WriteLine("-----------------\n");
    Console.WriteLine("This simple console app demonstrates the use of Action and Func generic delegates in C#.\n");
    PrintCode($"Code will appear in {Helpers.CODECOLOR.ToString()}.");
    PrintOutput($"Output will appear in {Helpers.OUTPUTCOLOR.ToString()}.\n\n");
}



// OutputToConsole and OutputToFile both satisfy the Action<string[]> contract, 
// and so either can be used. 
//
//
static void OutputToConsole(string[] words)
{
    // Since this is a delegates demonstration, let's use 
    // Array.ForEach<T>, which visits each item in the array
    // and passes its value to an Action delegate that, of course, takes a T. 
    // Here, our delegate just outputs the string to the console.
    //
    //Array.ForEach<string>(words, (w) => PrintOutput(w));
}


static void OutputWords(string[] words)
{
    PrintOutput("\nHere are the words sorted:\n");
    Array.ForEach<string>(words, (w) => PrintOutput(w));
}

// This must be static to be used as an action 
//
static int DoComparison(string a, string b)
{
    return a.CompareTo(b);
}


static void PrintCode(string thecode)
{
    Console.ForegroundColor = Helpers.CODECOLOR; 
    Console.WriteLine("\n" + thecode + "\n");
    Console.ResetColor();
}

static void PrintOutput(string output)
{
    Console.ForegroundColor = Helpers.OUTPUTCOLOR;
    Console.WriteLine(output);
    Console.ResetColor();
}



/// <summary>
/// 
/// </summary>
static class MyStringExtensions
{
    // Extension method for strings. 
    // Usage: string s; s.CountChars('-');
    //
    public static int CountChars(this string s, char c1)
    {
        return s.Count(c => c == c1);
    }

}

/// <summary>
/// 
/// </summary>
public class Employee : IComparable<Employee>
{
    public string LastName { get; set; } = "";
    public string ZipCode { get; set; } = "";

    // default sort order is LastName 
    public int CompareTo(Employee? other)
    {
        return LastName.CompareTo(other?.LastName);
    }
}

/// <summary>
/// Helpers static class. 
/// </summary>
public static class Helpers
{
    public static ConsoleColor OUTPUTCOLOR = ConsoleColor.Magenta;
    public static ConsoleColor CODECOLOR = ConsoleColor.Cyan;

    public delegate int CountTokensDelegateType(string s);

    public static int CountTokens(string s)
    {
        int tokens = 0;
        string[] words = s.Split(" ");

        // Add extra tokens for each syllable
        foreach (string word in words)
        {
            tokens += CountSyllables(word);  // call ChatGPT code 
        }
        char[] punctuation = { '"', '.', ',', '-', '\'', '"', ';', ':', '!', '@', '#', '$', '%', '&', '*', '(', ')', '+' };
        foreach (char c in punctuation)
        {
            tokens += s.CountChars(c);  // extension method. Counts the number of times c appears.
        }
        return tokens;
    }

    // Generated by ChatGPT
    public static int CountSyllables(string word)
    {
        // Remove non-letter characters. Convert the word to lowercase.
        string cw = Regex.Replace(word.ToLower(), @"[^a-z]", "");

        // Handle suffixes 
        string[] suffixes = { "es", "ed", "e" };
        foreach (string suffix in suffixes)
        {
            if (cw.EndsWith(suffix))
            {
                cw = cw.Substring(0, cw.Length - suffix.Length);
            }
        }

        // Count the number of vowels (a, e, i, o, u) in the cleaned word.
        int vowelCount = Regex.Matches(cw, "[aeiouy]").Count;

        // Adjust the count based on some patterns in consecutive vowels.
        vowelCount -= Regex.Matches(cw, "iou|ea$|e$").Count;

        // Ensure the minimum count is 1 (a word always has at least one syllable).
        return Math.Max(1, vowelCount);
    }

}
