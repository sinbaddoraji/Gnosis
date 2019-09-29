<main>
	var public cells = { "1", "2", "3", "4", "5", "6", "7", "8", "9"};
	var public player = "X";
	
	while(1 == 1)
	{
		pT;
		getInp;
	};
	
	
    pause;
</main>

<pT>
	cls;
	print << cells[0] << " | " << cells[1] << " | " << cells[2] << endl;
	print << cells[3] << " | " << cells[4] << " | " << cells[5] << endl;
	print << cells[6] << " | " << cells[7] << " | " << cells[8] << endl;
	print << endl;
</pT>

<getInp>
	print << "Choice : ";
	input >> input;
	
	if (input < 10 && input > 0)
	{
		cells[input-1] = player;
	};
	
	var played = 0;
	
	if (player == "X")
	{
		player = "O";
			
		played = 1;
	};
	
	if (player == "O" && played == 0)
	{
		player = "X";
			
		played = 0;
	};
</getInp>
