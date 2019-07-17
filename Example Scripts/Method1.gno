//This is a program showing the basic use of methods and variable scope
<main>
	var public str = "this is \"str\" from main";
	var str2 = "this is \"str2\" from main";
	print << str << endl;
	print << str2 << endl;
	method;
	
	print << str2 << endl;
	print << "This script shows how variable scopes can be manipulated" << endl;
	
    pause;
</main>

<method>
	var str = "this is \"str\" from Method";
	var str2 = "this is \"str2\" from Method";
	print << str << endl;
	print << str2 << endl;
	var public str2 = "blah blah";
</method>