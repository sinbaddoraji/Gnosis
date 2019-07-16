# Gnosis

Gnosis is an intepreter for a language Gnos. It is currently in development and is being written purley in c#.

Gnos makes use of a unique syntax. This is shown in the example below:

    <main>
        print << "Hello World!!" << " I am genos reborn" << endl;
        input "What is your name? " >> name;
        print << "Hello " << name << endl;
        pause;
    </main>


There are 5 variable types: double, float, long, int, bool and string. Variable declearation and use is dynamic. The "var" keyword is used to initialize variables which are decided automatically by the intepreter. This is displayed in the example below

    <main>
        var value = 4 + 5 + 6;
        var value2 = value + 1;
        var str = "\nHello strings work too " + value2 + " Not in a strict way either";

        print << value + " " + value2<< endl;
        print << str << endl;

        pause;
    </main>
    
   
Basic Keywords:
	print   input   var     pause
	while   switch  for     foreach
	if      case    let



The following are example cases on the use of the stated tokens

input:
	<main>
		input >> variable;
		print << variable << endl;
		input "Tell me something: " >> variable;
		print << variable << endl;
        </main>
	
var:
	<main>
		var name = "yesa";
		var elements = {"air", "fire", "earth", "water"};
		
		print << name << endl;
		for (var i = 0| i < $elements| i++)
		{
			print << elements[i] << endl;
		};

		pause;
	</main>
	
short hand code like += -= *= /= %=

	<main>
		var name = value;
		var name += value + valueMethod();
        </main>

string:
	Escape Characters:

	\a	Bell or alert
	\b	Backspace
	\f	Formfeed
	\n	Newline
	\r	Carriage return
	\t	Tab
	\v	Vertical tab
	
	Strings are to be placed in-between double quotes. Example: "This is a string"
	
	<main>
		var str = "\nHello strings work too " + value2 + " Not in a strict way either";
	</main>
	
operators:
	  "+", "-","*", "/", "=", ">", "<", "!", "%", "[", "]", "{", "}", "(", ")", ",", ";"
	  "++", "--", "+=", "-=", "*=", "/=", "!=", "==", "<<", ">>", "||", "&&"
	  

Inbuilt methods:
	pause()
	pause(value="String to be displayed while paused")
	
	split(variable,ArrayName,splitCharacter)
	split(variable,ArrayName,splitString)
	
	join(Array variable,Joining string) // Always return as string
	
	replace(variable,a,b)
	remove(variable,start,stop)
	swap(variable,a,b)
	reverse(variable)
	
	lower(string variable)
	upper(string variable)
	
	
	eval(expression)//solve expression
	
	date// Return as string
	time// Return as string
	year// Return as string
	day// Return as string
	month// Return as string
	
	
	
IO Handling:



Writing Files:

	write FileName WriteMode << Value << value;

WriteModes: Overwrite, Append


Reading Files:
	
	read Filename >> variable; // As string
	

Deleting Files:
	
	delete Filename;
	
Renaming Files:

	rename Filename NewFilename
	
Change Working Directory:
	
	cd DirectoryName
	
Make  Directory:

	mkdir DirectoryName
	
	
	

Console Manipulation:


Change Foreground:

	fg Colour

Change Background:

	bg Colour
	
Change Title:

	title string
	

	
Function Manipulation:

Run code from string:

	Work string


Unique features:
	modifiable variable types : a string variable can become an integer later
	dynamic arrays : values can be added to arrays infinitly using +=
	
