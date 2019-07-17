<main>
	print << "(1) Addition" << endl;
	print << "(2) Subtraction" << endl;
	print << "(3) Multiplication" << endl;
	print << "(4) Division" << endl;
	
	print << "Choice : ";
	input public >> inp;
	
	var public add = (inp == 1);
	var public sub = (inp == 2);
	var public mul = (inp == 3);
	var public div = (inp == 4);
	var public none = (add == false && sub == false && mul == false && div == false);
	
	if (none == false)
	{
		print << " : " << exp << endl;
	};
	
	if (none)
	{
		print << "invalid option" << endl;
	};
	
	
    pause;
</main>

<exp>
		print << "Your choice was: " << inp << endl << endl;
		input " First Number: " >> a;
		input "Second Number: " >> b;
		
		var c = 0;
		
		if (add)
		{
			c = a + b;
		};
		
		if (sub)
		{
			c = a - b;
		};
		
		if (mul)
		{
			c = a * b;
		};
		
		if (div)
		{
			c = a / b;
		};
	
		return c;
</exp>