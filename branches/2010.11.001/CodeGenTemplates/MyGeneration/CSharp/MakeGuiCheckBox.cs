	public GuiCheckBox MakeGuiCheckBox(string name,string caption,bool def, string helptext,int width,GuiCheckBox related,int offX, int offY)
	{
			GuiCheckBox tmp = ui.AddCheckBox( name, caption, def, helptext );
			tmp.Width=width;
			tmp.Top=related.Top+(offY<0?-offY * related.Height:offY);
			tmp.Left=related.Left+(offX<0?-offX * related.Width:offX);
			return tmp;
	}
	public GuiCheckBox MakeGuiCheckBox(string name,string caption,bool def, string helptext,int width)
	{
			GuiCheckBox tmp = ui.AddCheckBox( name, caption, def, helptext );
			tmp.Width=width;
			return tmp;
	}
