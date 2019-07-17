<main>
	print << "(1) Addition" << endl;
	print << "(2) Subtraction" << endl;
	print << "(3) Multiplication" << endl;
	print << "(4) Division" << endl;
	
	print << "Choice : ";
	input >> inp;
	
	input " First Number: " >> a;
	input "Second Number: " >> b;
	
	if (inp == 1)
	{
		var c = a + b;
		print << " : " + c << endl;
	};
	
	if (inp == 2)
	{
		var c = a - b;
		print << " : " + c << endl;
	};
	
	if (inp == 3)
	{
		var c = a * b;
		print << " : " + c << endl;
	};
	
	if (inp == 4)
	{
		var c = a / b;
		print << " : " + c << endl;
	};
	
    pause;
</main>