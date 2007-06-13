	private string FKSelectName(ITable tbl,IForeignKey FK,string sAlias)
	{
		if(tbl.Name == FK.PrimaryTable.Name)return ChildName(FK) + ClassesName(tbl.Name);
		//return ClassesName(tbl.Name) + "By" + ClassName(FK.PrimaryTable) + sAlias;
		return ClassesName(tbl.Name) + "By" + ForeignKeyName(FK) + sAlias;
	}
	private string ForeignKeyName(IForeignKey FK)
	{
		string retval="";
		string sep = "";
		
		foreach(IColumn col in FK.ForeignColumns)
		{
			retval = retval + sep + ClassName(col.Name);
			sep = "_And_";
		}
		return retval;
	}
	private string FKFieldName(IForeignKey fk)
	{
		string cName = ClassName( fk.ForeignTable );
		string className=ClassName( fk.PrimaryTable );
		if(className==cName)return ChildName(fk);
		return ClassName( fk.ForeignTable );
	}
	private string FKFieldsName(IForeignKey fk)
	{
		string cName = ClassesName( fk.ForeignTable );
		string className=ClassesName( fk.PrimaryTable );
		if(className==cName)return ChildrenName(fk);
		return cName;
	}
	private string FKClassesName(IForeignKey fk)
	{
		string cName = ClassesName( fk.ForeignTable );
		string className=ClassesName( fk.PrimaryTable );
		if(className==cName)return ChildName(fk) + ClassesName( fk.PrimaryTable );
		return ClassName( fk.PrimaryTable ) + cName;
	}
	private string FKClassName(string className,IForeignKey fk)
	{
		string cName = ClassName( fk.ForeignTable );
		if(className==cName)return ChildName(fk) + cName;
		return className + cName;
	}
	private string FKClassName(IForeignKey fk)
	{
		string className=ClassName(fk.PrimaryTable);
		return FKClassName(className,fk);
	}
	private string FKBy(IForeignKey fk)
	{
		if(fk.PrimaryTable.Name==fk.ForeignTable.Name)return ChildrenName(fk);
		return "By" +FormatColumns("{prop}",fk.ForeignColumns,"_","");
	}
	private string FKAndString(string className,IForeignKey fk,string str)
	{
		string cName = ClassName( fk.ForeignTable );
		if(className==cName)return str;
		return "";
	}
	private string FKCountName(ITable tbl,IForeignKey FK,string sAlias)
	{
		if(tbl.Name == FK.ForeignTable.Name)return ChildName(FK);
		return ClassName(FK.ForeignTable) + sAlias;
	}
	private bool FKParent(ITable tbl)
	{
		foreach(IForeignKey fk in tbl.ForeignKeys)
		{
			if(fk.PrimaryTable == fk.ForeignTable)return true;
		}
		return false;
	}
	private IForeignKey FKParentFK(ITable tbl)
	{
		foreach(IForeignKey fk in tbl.ForeignKeys)
		{
			if(fk.PrimaryTable == fk.ForeignTable)return fk;
		}
		return null;
	}
	private bool IsParentCol(IColumn col)
	{
		IForeignKey fk = FKParentFK(col.Table);
		if(fk==null)return false;
		foreach(IColumn col1 in fk.ForeignColumns)
		{
			if(col.Name==col1.Name)return true;
		}
		return false;
	}
	private string parentName(IColumn col)
	{
		IColumn colp = ParentCol(col);
		if (colp == null)
		{
			if(IsRelObj(col))return LocalName(RelObjProp(col)) + "." + RelObjCol(col);
			return LocalName(RelObjProp(col));
		}
		return "my" + ParentName(col.ForeignKeys[0]) + "." + PropertyName(colp.Name);
	}
	private string ParentName(IColumn col)
	{
		IColumn colp = ParentCol(col);
		if (colp == null)
		{
			if(IsRelObj(col))return PropertyName(col);
			return MemberName(RelObjProp(col));
		}
		return PropertyName(col.Name);
	}
	private string ParentRef(IColumn col)
	{
		IColumn colp = ParentCol(col);
		if (colp != null) return ClassName(col.Table) + ".Get(" + RelObjTypeCast(col) + MemberName(col) + ")";
		return null;
	}
	private string ParentTypeName(IColumn col)
	{
		IColumn colp = ParentCol(col);
		if (colp != null) return ClassName(col.Table) + " myParent";
		return null;
	}
	private IColumn ParentCol(IColumn col)
	{
		IForeignKey fk = FKParentFK(col.Table);
		if(fk==null)return null;
		return RelatedColumnF(col,fk);
	}
	private bool IsParentColumn(IColumn col)
	{
		IForeignKey fk = FKParentFK(col.Table);
		if(fk==null)return false;
		foreach(IColumn colf in fk.ForeignColumns)
			if(colf.Name == col.Name)return true;
		return false;
	}
	private string AutoSeed(IColumn col)
	{
		IForeignKey fk = FKParentFK(col.Table);
		if(fk==null)return null;
		IColumn colp = RelatedColumnF(col,fk);
		if(colp==null)return "";
		return (colp.IsAutoKey?colp.AutoKeySeed.ToString():"");
	}
	
	private IColumn RelatedColumn(IColumn column,IForeignKey fk){
		if(column.Table.Name == fk.PrimaryTable.Name && fk.ForeignColumns.Count == 1){
			return fk.ForeignColumns[0];
		}
		if(column.Table.Name == fk.ForeignTable.Name && fk.PrimaryColumns.Count == 1){
			return fk.PrimaryColumns[0];
		}
		return null;
	}
	private IColumn RelatedColumnF(IColumn col,IForeignKey fk){
		for(int i =0; i < fk.PrimaryColumns.Count;i++) // Loop through the related columns
		{
			IColumn pcol=fk.PrimaryColumns[i]; // This is the primary column
			IColumn fcol=fk.ForeignColumns[i]; // This is the foreign column
			//if(pcol.Name == col.Name && pcol.Table.Name == col.Table.Name)return fcol;
			if(fcol.Name == col.Name && fcol.Table.Name == col.Table.Name)return pcol;
		}
		return null;
	}
	private IColumn RelatedColumnP(IColumn col,IForeignKey fk){
		for(int i =0; i < fk.PrimaryColumns.Count;i++) // Loop through the related columns
		{
			IColumn pcol=fk.PrimaryColumns[i]; // This is the primary column
			IColumn fcol=fk.ForeignColumns[i]; // This is the foreign column
			if(pcol.Name == col.Name && pcol.Table.Name == col.Table.Name)return fcol;
			//if(fcol.Name == col.Name && fcol.Table.Name == col.Table.Name)return pcol;
		}
		return null;
	}
	private IColumn RelatedColumnB(IColumn col,IForeignKey fk){
		for(int i =0; i < fk.PrimaryColumns.Count;i++) // Loop through the related columns
		{
			IColumn pcol=fk.PrimaryColumns[i]; // This is the primary column
			IColumn fcol=fk.ForeignColumns[i]; // This is the foreign column
			if(pcol.Name == col.Name && pcol.Table.Name == col.Table.Name)return fcol;
			if(fcol.Name == col.Name && fcol.Table.Name == col.Table.Name)return pcol;
		}
		return null;
	}
	private string ProcessName(Match m)
	{
		return m.Value.TrimStart("-_ ".ToCharArray()).ToUpper();
	}
	private string Suffix(IColumn col)
	{
		//return (PropertyName(col.Name)==_className?"Fld":"");
		return (PropertyName(col.Name)==ClassName(col.Table.Name)?"Fld":"");
	}
	private string PropertyName(IColumn col)
	{
		return PropertyName(col.Name)+Suffix(col);
	}
	private string PropertyName(string name)
	{
		return Regex.Replace(name, "^[a-z]|[-_ ][a-zA-Z0-9]",new MatchEvaluator(ProcessName));		
	}
	private bool MixedCase(string s)
	{
		bool hasUpper = false;
		bool hasLower = false;
		foreach (char c in s.ToCharArray())
		{
			hasUpper |= Char.IsUpper(c);
			hasLower |= char.IsLower(c);
			if (hasLower && hasUpper) return true;
		}
		return false;
	}
	private string MemberName(string name)
	{
		return _prefix + name;
	}
	private string MemberName(IColumn c)
	{
		return MemberName(PropertyName(c));
	}
	private string LocalName(IColumn col)
	{
		return LocalName(col.Name)+Suffix(col);
	}
	private string LocalName(string s)
	{
		s=PropertyName(s);
		if(MixedCase(s))return ToLeadingLower(s);
		else return s.ToLower();
	}
	private string ClassName(string name)
	{
		return SingularName(PropertyName(name));
	}
	private string ClassName(ITable table)
	{
		return ClassName(table.Alias);
	}
	private string ClassName(IView view)
	{
		return ClassName(view.Alias);
	}
	private string ClassName(IColumn column)
	{
		if(column.Table != null)return ClassName(column.Table);
		if(column.View != null)return ClassName(column.View);
		return null;
	}
	private string ClassesName(string name)
	{
		return PluralName(PropertyName(name));
	}
	private string ClassesName(ITable table)
	{
		return ClassesName(table.Alias);
	}
	private string ClassesName(IView view)
	{
		return ClassesName(view.Name);
	}
	private string ClassesName(IColumn column)
	{
		if(column.Table != null)return ClassesName(column.Table);
		if(column.View != null)return ClassesName(column.View);
		return null;
	}
	private string SingularName(string s)
	{
		if(Regex.IsMatch(s,"crises$"))return Regex.Replace(s,"crises$","crisis");
		if(Regex.IsMatch(s,"uses$"))return Regex.Replace(s,"uses$","us");
		if(Regex.IsMatch(s,"is$"))return s;
		if(Regex.IsMatch(s,"us$"))return s;
		if(Regex.IsMatch(s,"sses$"))return Regex.Replace(s,"sses$","ss");
		if(Regex.IsMatch(s,"ches$"))return Regex.Replace(s,"ches$","ch");
		if(Regex.IsMatch(s,"ies$"))return Regex.Replace(s,"ies$","y");
		if(Regex.IsMatch(s,"ss$"))return s;
		return Regex.Replace(s,"s$","");
	}
	private string PluralName(string s)
	{
		s=SingularName(s);
		if(Regex.IsMatch(s,"crisis$"))return Regex.Replace(s,"crisis$","crises");
		if(Regex.IsMatch(s,"us$"))return Regex.Replace(s,"us$","uses");
		if(Regex.IsMatch(s,"ises$"))return s;
		if(Regex.IsMatch(s,"uses$"))return s;
		if(Regex.IsMatch(s,"ss$"))return Regex.Replace(s,"ss$","sses");
		if(Regex.IsMatch(s,"ch$"))return Regex.Replace(s,"ch$","ches");
		if(Regex.IsMatch(s,"y$"))return Regex.Replace(s,"y$","ies");
		return s + "s";
	}
	private string ReturnType(string type)
	{
		if(type=="SmartDate")type="string";
		return type;
	}
	private string ReturnMember(string type)
	{
		string member="";
		return member;
	}
	private string CSLAType(IColumn column)
	{
		return CSLAType(column,(column.IsNullable? "?":""));
	}
	private string CSLAType(IColumn column,string suffix)
	{
		string type = column.LanguageType;
		switch( column.LanguageType )
		{
			case "DateTime":
				if(column.Description.IndexOf("{datetime}")>=0)
					type="DateTime" + suffix;
				else
					type = "SmartDate";
				break;
			case "short":
				type="Int16" + suffix;
				break;
			case "string":
				break;
			case "byte[]":
				break;			default:
				type += suffix;
				break;
	//		case "uint":
	//			retVal = "int";
	//			break;
	//		case "ulong":
	//			retVal = "long";
	//			break;
	//		case "ushort":
	//			retVal = "short";
	//			break;
			
		}
		return type;
	}
	string DefaultValue(IColumn column)
	{
		string s = RemoveParens(column.Default);
		switch(s)
		{
			case "getdate()":
				if(CSLAType(column)=="DateTime")
					s="DateTime.Now";
				else
					s="DateTime.Now.ToShortDateString()";
				break;
			case "upper(suser_sname())":
				s= "Environment.UserName.ToUpper()";
				break;
			case "suser_sname()":
				s="Environment.UserName";
				break;
			default:
				if(IsNumeric(s))s="" + s;
				else s="";
				break;
		}
		return s;
	}
	private string InitializeValue( IColumn Column )
	{
		string retVal=";";
		if(Column.DataTypeName=="timestamp")
		{
			retVal = " = new byte[8];//timestamp";
		}
		else
		{
			//if(Column.Default != ""){
			//	retVal = ConvertDefault(Column) + ";// TODO: Default from DB " + RemoveParens(Column.Default) + " ";
			//}
			//else
			//{
				switch( CSLAType(Column ) )
				{
					case "string":
						retVal = " = string.Empty;";
						break;
					case "DateTime":
						retVal = " = new DateTime();";
						break;
					case "SmartDate":
						retVal = " = string.Empty;";
						break;
					//case "Guid":
					//	retVal = "=new Guid();";
					//	break;
					default:
						// nothing to do here
						break;
				}
			//}
		}
		return retVal;
	}
	public static bool IsNumeric(string stringToTest) 
	{ 
		double newVal; 
		return double.TryParse(stringToTest, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out newVal); 
	} 
	private string RemoveParens(string s)
	{
		while(s.StartsWith("(") && s.EndsWith(")"))
			s=s.Substring(1,s.Length-2);
		return s;
	}
	private string ParameterName(IColumn column)
	{
		return "@" + PropertyName(column.Name);
	}
	private string NewParameterName(IColumn column)
	{
		return "@new" + PropertyName(column.Name);
	}
	private string ParamKeyName(IColumn column)
	{
		if(column.IsAutoKey)return NewParameterName(column);
		return ParameterName(column);
	}
	private string ToLeadingLower( string name )
	{
		char[] chars = name.ToCharArray();
		chars[0] = Char.ToLower( chars[0] );
		return new string( chars );
	}
	private string DBType(IColumn column)
	{
		string s=column.DataTypeNameComplete;
		switch(s){
			case "text":
				s="varchar(MAX)";
				break;
			case "ntext":
				s="nvarchar(MAX)";
				break;
		}
		return s;
	}
// Old ----------------------------------------------
//	private string ToClassName(string name)
//	{
//		return Regex.Replace(ToPascalCase(name),"s$","");
//	}
//	private string ColumnToMemberVariable( IColumn Column )
//	{
//		return _prefix + UniqueColumn( Column ).ToLower();
//	}
//	
//	private string ColumnToPropertyName( IColumn Column )
//	{
//		return ToPascalCase( UniqueColumn( Column ) );
//	}
