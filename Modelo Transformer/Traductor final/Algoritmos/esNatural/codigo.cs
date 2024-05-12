public virtual int esNatural (int n)
{
    if (n <= 0) 
    throw new ArgumentException("n debe ser un numero positivo");

    else if (n > 0)
		Console.WriteLine("El numero introducido es un numero natural");
}

