<main>
	var a = 1;
	
	while (a < 101) {
        var d = a%3;
		var v = a%5;

		if (d == 0 && v == 0)
		{
			print << "fizz buzz" << endl;
		};
		
		if (v == 0)
		{
			print << "fizz" << endl;
		};
		
		if (d == 0)
		{
			print << "buzz" << endl;
		};
		
		a += 1;
    };
	
    pause;

</main>