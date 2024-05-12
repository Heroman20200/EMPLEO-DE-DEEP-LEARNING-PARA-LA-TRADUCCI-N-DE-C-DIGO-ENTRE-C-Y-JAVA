public virtual int METHOD_NAME(int VAR_INT)
    {
        if (VAR_INT <= LITERAL_INT)
            throw new ArgumentException(LITERAL_STRING);
        else if (VAR_INT > LITERAL_INT)
            Console.WriteLine(LITERAL_STRING);
    }