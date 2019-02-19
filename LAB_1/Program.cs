using System;
using System.Collections.Generic;
using System.Reflection;

///Лабораторная работа №1 Кравцоваой А. В.///

namespace LAB_1
{
	class Program
	{
		class MethInf
		{
			public int Overloads { get; set; }
			public int Minarg { get; set; }
			public int Maxarg { get; set; }
			public MethInf(int arg)
			{
				Overloads = 1;
				Minarg = arg;
				Maxarg = arg;
			}

			public void UpdMethInfo(int arg)
			{
				++Overloads;
				if (arg < Minarg) { Minarg = arg; }
				if (arg > Maxarg) { Maxarg = arg; }
			}
		}



		//тестовый класс и структура для проверки рефлексии на пользовательских типах//
		class MyClass
		{
			public double d;
			public static int i;
			//public int E { get; set; }
			public MyClass(double d) { this.d = d; }
			public void MyMeth(int a, int b, int c) { }
			public void MyMeth() { }

		}
		struct MyStruct
		{
			public double d;
			public static int i;
		}

		/////////METHODS://////////

		static void Main(string[] args)
		{
			StartMenu();
		}


		static void StartMenu()
		{
			Console.WriteLine(@"
Выберете необходимое действие:
1 – Задача №1
2 – Выход");
			char c = Console.ReadKey(true).KeyChar;
			switch (c)
			{
				case '1':
					InfoProgram();
					break;
				default:
					return;
			}

		}

		////////////////////////////
		static void InfoProgram()
		{
			while (true)
			{
				Console.WriteLine(@"
Выберете необходимое действие:
a – Общая информация по типам
b – Выбрать из списка
c – Ввести имя типа
d – Параметры консоли
e - Выход из программы");

				Type t;

				switch (char.ToLower(Console.ReadKey(true).KeyChar))
				{
					case 'a':
						AllAboutTypes();
						break;

					case 'b':
						t = SelectType();
						if (t != null)
						{ ShowInfo(t); }
						break;
					case 'c':
						t = InputType();
						if (t != null)
						{ ShowInfo(t); }
						break;
					case 'd':
						Grounds();
						break;
					case 'e': return;
					default: break;
				}

			}

		}

		////////////////////////////

		static Type SelectType()
		{

			Console.WriteLine(@"
Выберете тип из списка:
1 – uint
2 – int
3 – long
4 – float
5 – double
6 – char
7 - string
8 – MyClass
9 – MyStruct
0 – Выход в главное меню");

			char c = Console.ReadKey(true).KeyChar;
			switch (c)
			{
				case '1':
					return typeof(uint);
				case '2':
					return typeof(int);
				case '3':
					return typeof(long);
				case '4':
					return typeof(float);
				case '5':
					return typeof(double);
				case '6':
					return typeof(char);
				case '7':
					return typeof(string);
				case '8':
					return typeof(MyClass);
				case '9':
					return typeof(MyStruct);
				default: return null;

			}
		}

		////////////////////////////
		static Type InputType()
		{

			Console.WriteLine(@"
Введите имя типа:");

			string s = Console.ReadLine();
			switch (s.Trim().ToLower())
			{
				case "uint":
				case "uint32":
					return typeof(uint);
				case "int":
				case "int32":
					return typeof(int);
				case "long":
				case "int64":
					return typeof(long);
				case "float":
				case "single":
					return typeof(float);
				case "double":
					return typeof(double);
				case "char":
					return typeof(char);
				case "string":
					return typeof(string);
				case "myclass":
					return typeof(MyClass);
				case "mystruct":
					return typeof(MyStruct);
				default:
					Console.WriteLine("Типа не существует!");
					return null;

			}
		}
		////////////////////////////
		static void ShowInfo(Type t)
		{
			Console.WriteLine("\n\nИнформация по типу:    " + t.FullName + "\n");//{0,20} по правому краю
			Console.WriteLine("Значимый тип:              " + (t.IsValueType ? "+" : "-"));
			Console.WriteLine("Пространство имен:         " + t.Namespace);
			Console.WriteLine("Сборка:                    " + t.Assembly.GetName().Name);
			Console.WriteLine("Общее число элементов:     " + t.GetMembers().Length);
			Console.WriteLine("Число методов:             " + t.GetMethods().Length);
			Console.WriteLine("Число свойств:             " + t.GetProperties().Length);
			Console.WriteLine("Число полей:               " + t.GetFields().Length);



			FieldInfo[] fields = t.GetFields();
			string allfields = "";
			for (int i = 0; i < fields.Length; ++i)
			{
				allfields += fields[i].Name;
				allfields += " ";
			}
			Console.WriteLine("Список полей: " + (allfields != "" ? allfields : "--"));

			PropertyInfo[] properties = t.GetProperties();
			string allproperties = "";
			for (int i = 0; i < properties.Length; ++i)
			{
				allproperties += properties[i].Name;
				allproperties += " ";
			}
			Console.WriteLine("Список свойств: " + (allproperties != "" ? allproperties : "--"));
			Console.WriteLine(@"Нажмите M для вывода дополнительной информации по
методам:
Нажмите 0 для выхода в главное меню:");
			char c = Console.ReadKey(true).KeyChar;
			switch (char.ToLower(c))
			{
				case 'm':
					AllAboutMethods(t);
					break;
				default:
					return;
			}

			Console.ReadKey();
		}

		////////////////////////////
		static void AllAboutMethods(Type t)
		{
			MethodInfo[] methods = t.GetMethods();
			SortedDictionary<string, MethInf> s_dic = new SortedDictionary<string, MethInf>();
			for (int i = 0; i < methods.Length; ++i)
			{
				if (!(s_dic.ContainsKey(methods[i].Name)))
				{
					s_dic.Add(methods[i].Name, new MethInf(methods[i].GetParameters().Length));
				}
				else
				{
					s_dic[methods[i].Name].UpdMethInfo(methods[i].GetParameters().Length);
				}
			}

			Console.WriteLine($"\nМетоды типа {t.FullName}\n");
			Console.WriteLine($@"Название:      Число перегрузок:      Число параметров:");

			foreach (var key_value in s_dic)
			{
				Console.WriteLine($"{key_value.Key,-30}{key_value.Value.Overloads,-20}{key_value.Value.Minarg}" +
						((key_value.Value.Maxarg != key_value.Value.Minarg) ? $"..{key_value.Value.Maxarg}" : ""));
			}
		}


		////////////////////////////
		static void AllAboutTypes()
		{
			Assembly[] refAssemblies = AppDomain.CurrentDomain.GetAssemblies();
			// лист типов
			List<Type> types = new List<Type>();
			foreach (Assembly asm in refAssemblies)
			{
				types.AddRange(asm.GetTypes());
			}
			//ссылочные VS значимые
			int v_types = 0;
			int r_types = 0;

			//Типы - интерфейсы
			int i_f = 0;
			//Тип с максимальным числом методов:
			int max_methods = 0;
			string max_methods_name = "";
			//Самое длинное название метода:
			int length_of_name = 0;
			string longest_name = "";
			//Метод с наибольшим числом аргументов:

			int max_arg = 0;
			string max_arg_meth = "";
			string max_arg_meth_type = "";


			foreach (var t in types)
			{
				if (t.IsValueType)
				{ ++v_types; }

				else
				{ ++r_types; }


				if (t.IsInterface)
				{
					//    i_face += t.Name;
					//    i_face += " ";
					++i_f;
				}

				MethodInfo[] methods = t.GetMethods();
				if (methods.Length > max_methods)
				{
					max_methods = methods.Length;
					max_methods_name = t.Name;
				}
				foreach (var m in methods)
				{
					if (m.Name.Length > length_of_name)
					{
						length_of_name = m.Name.Length;
						longest_name = m.Name;
					}

					int temp_arg = m.GetParameters().Length;
					if (temp_arg > max_arg)
					{
						max_arg = temp_arg;
						max_arg_meth = m.Name;
						max_arg_meth_type = t.Name;
					}
				}
			}

			Console.WriteLine($@"

Общая информация по типам: 

Подключенные сборки:{refAssemblies.Length}
Всего типов по всем подключенным сборкам:{types.Count}
Ссылочные типы: {r_types}
Значимые типы: {v_types}
Типы - интерфейсы: {i_f}
Тип с максимальным числом методов: {max_methods_name} ({max_methods})
Самое длинное название метода:{longest_name} ({length_of_name})
Метод с наибольшим числом аргументов: {max_arg_meth} ({max_arg}) (относится к классу: {max_arg_meth_type})

Нажмите любую клавишу, чтобы вернуться в главное меню");

		}



		///Цвет консоли ///

		static void Grounds()
		{

			Console.WriteLine(@"
Выберите параметры консоли:
1 - желтый с бирюзовым
2 - зеленый с розовым
3 - синий с голубым
4 - вернуть цвета о умолчанию
");

			switch (Console.ReadKey(true).KeyChar)
			{
				case '1':
					Console.BackgroundColor = ConsoleColor.Yellow;
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Clear();
					return;
				case '2':
					Console.BackgroundColor = ConsoleColor.Green;
					Console.ForegroundColor = ConsoleColor.Magenta;
					Console.Clear();
					return;
				case '3':
					Console.BackgroundColor = ConsoleColor.DarkBlue;
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.Clear();
					return;
				case '4':
					Console.ResetColor();
					Console.Clear();
					return;
				default:
					return;
			}

		}

	}
}

