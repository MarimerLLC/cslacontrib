	private void WriteLine(string format,params object [] args)
	{
		output.writeln(string.Format(format,args));
	}
	private void Write(string format,params object [] args)
	{
		output.write(string.Format(format,args));
	}
	private void WriteProp(object arg)
	{
		WriteProp(",",arg,"");
	}
	private void WriteProp(string prefix,object arg,string suffix)
	{
		try
		{
			if(arg.GetType()==typeof(string))Write("{0}\"{1}\"{2}",prefix,arg,suffix);
			else if(arg.GetType().IsPrimitive)Write("{0}{1}{2}",prefix,arg,suffix);
			else Write("{0}\"{1}\"{2}",prefix,arg,suffix);
		}
		catch(Exception)
		{
			Write("{0}{2}",prefix,arg,suffix);
		}
	}
	

