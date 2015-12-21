<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var input = 29000000;

	var guess = 0;
	while(PresentsForHouse(guess) < input)
		guess += 1000 ;
	var house = guess;
	for (var i = 0; i< 50000; i++)
		if (PresentsForHouse(guess-i) >= input)
			house = guess-i;
	
	house.Dump();
	
	
	guess = 0;
	while(PresentsForHouse2(guess) < input)
		guess += 1000 ;
	house = guess;
	for (var i = 0; i< 50000; i++)
		if (PresentsForHouse2(guess-i) >= input)
			house = guess-i;
	
	house.Dump();

}

int PresentsForHouse(int house)
{
	var presents = 0;
	for(var elf = 1; elf <= house; elf++)
		if ((house % elf) == 0)
			presents += (elf * 10);
	return presents;
}

int PresentsForHouse2(int house)
{
	var presents = 0;
	for(var elf = 1; elf <= house; elf++)
		if ((house % elf) == 0 && (house / elf) <= 50)
			presents += (elf * 11);
	return presents;
}