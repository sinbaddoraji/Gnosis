<main>
	print << "(1) Addition" << endl;
	print << "(2) Subtraction" << endl;
	print << "(3) Multiplication" << endl;
	print << "(4) Division" << endl;
	
	print << "Choice : ";
	input >> inp;
	
	var add = (inp == 1);
	var sub = (inp == 2);
	var mul = (inp == 3);
	var div = (inp == 4);
	var none = (add == false && sub == false && mul == false && div == false);
	
	if (none == false)
	{
		input " First Number: " >> a;
		input "Second Number: " >> b;
		
		if (add)
		{
			var c = a + b;
			print << " : " + c << endl;
		};
		
		if (sub)
		{
			var c = a - b;
			print << " : " + c << endl;
		};
		
		if (mul)
		{
			var c = a * b;
			print << " : " + c << endl;
		};
		
		if (div)
		{
			var c = a / b;
			print << " : " + c << endl;
		};
	};
	
	if (none)
	{
		print << "invalid option" << endl;
	};
	
    pause;
</main>