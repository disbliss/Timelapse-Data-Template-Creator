   M S B A M L   `   `   `  ����R  NTimelapseTemplateEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:TimelapseTemplateEditorTimelapseTemplateEditor  5   	   (    "TimelapseTemplateEditor.MainWindow   5   	   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   8x,http://schemas.microsoft.com/winfx/2006/xaml    5   	   ;toolkit-http://schemas.microsoft.com/wpf/2008/toolkit  5   	   1local%clr-namespace:TimelapseTemplateEditor  5   	   -   5        $��
OnlyWindow��5   	     � Title$  Timelapse Template Editor��5   	   $	��700��6+   $
��1100��68   $	��560��6E   $	��800��6U    � 	Resources 5         DefaultValueToolTip  LabelToolTip  DataLabelToolTip  TooltipToolTip  WidthToolTip  CopyableToolTip  VisibleToolTip  TypeNoteToolTip  TypeCounterToolTip 	 TypeFixedChoiceToolTip 
 TypeFileToolTip  TypeFolderToolTip  TypeDateToolTip  TypeTimeToolTip  TypeImageQualityToolTip  ListFixedChoiceToolTip6   0TimelapseTemplateEditor.ListBoxDBOutputConverter  listConverter4   .TimelapseTemplateEditor.CellTextBlockConverter  cellTBConverter  RadioButtonCodeBar �� 
TargetType �� BasedOn �� Property �� Value �� 
TargetType ��Name �� Padding	 �� CornerRadius
 J� SnapsToDevicePixels ��Name �� Triggers P� Property P� Value �� 
TargetName  LabelCodeBar  TextBoxCodeBar  ComboBoxCodeBar  StackPanelCodeBar  ComboBoxListContextMenu
 � Tag m� Icon��%D  &	        &	 t     &	 �     &	     &	 �    &	 a    &	     &	 �    &	 �    &		 \    &	
     &	 �    &	 �    &	 �    &	 $    &	 �    &	 8    &	 K    &	 ^    7d�&	 �
    &	 �    &	 ;    &	     &	 �    _� 5	   
   .��5
      �� FDAn optional default data value that will be displayed to the analyst6^   5      _� 5   
   .��5      �� ?=The text describing this field as shown in the user interface6W   5      _� 5   
   .��5      �� pnThe text describing this field as written in the output file. Will be the same as the Label if its left empty.6�   5      _� 5   
   .��5      �� DBThe help text describing this field as shown in the user interface6\   5      _� 5   
   .��5      �� B@The width of the data entry field as shown in the user interface6Z   5      _� 5   
   .��5      �� ��Whether the data value from the former image will be copied to the current image if the user selects the ‘Copy’ button in Timelapse6�   5      _� 5   
   .��5      �� ;9Whether this item will be displayed in the user interface6S   5      _� 5   
   .��5      �� &$The Note item accepts free-form text6>   5       _� 5!   
   .��5"      �� WUThe Count item provides a point and click interface for counting entities in an image6o   5#      _� 5$   
   .��5%      �� zxThe Choice item provides a drop down menu of possible choices that can be entered. Edit the list to create these choices6�   5&      _� 5'   
   .��5(      �� XVThe File Item is filled in automatically by the system with the file name of the image6p   5)      _� 5*   
   .��5+      �� :8The Folder Item is filled in automatically by the system6R   5,      _� 5-   
   .��5.      �� kiThe Date item is filled in automatically by the system but will still be editable if changes are required6�   5/      _� 50   
   .��51      �� kiThe Time item is filled in automatically by the system but will still be editable if changes are required6�   52      _� 53   
   .��54      �� A?The Image Quality item is filled in automatically by the system6Y   55      _� 56   
   .��57      �� wuA list of items displayed as a menu of choices. Type in new items; right click on an item to edit, move or delete it.6�   58        59   
   6)     5:   
   6'   �� 5<   
   " 	�6,   8   .�5=      ��  � �� 5>      �� 5?      " d�6&   .R�5@      �� $ border��6X   $��Right=�6!   $��Center=�6=   $ 5,5,5,5q�6h   $	 5,5,5,5��6z   
�������
����ح�$��1,1,1,4q�6�   
 . ._�5A      �� $ contentPresenter��6/   5B       5D      P� 5E       �@d�	IsChecked$
 trueY�6;   . �5F   "   �� $ border��6?    � ��
 ��ح�6S   �� 5G   "   $ border��6@    � ��
 �����6T   5H      5J      5L      �� 5M       � ��$
 Bold"�6+   �� 5N       � ��$ 0,0,2,0q�6'   5O      �� 5Q   
   " ��6&   .�5R      ��  � ��$ Right=�64   �� 5S       � ��$ Center=�62   �� 5T       � ��$ 5,5,5,5q�6(   �� 5U       � ��$
 Bold"�6+   �� 5V       � ��$ 30��6'   5W      �� 5Y   
   " ��6(   .�5Z      ��  � ��$
 Left=�64   �� 5[       � ��$ Center=�62   �� 5\       � ��$ 0,4,5,5q�6(   �� 5]       � ��$ 1q�60   �� 5^       � ��
 ��ح�6,   �� 5_       � ��$ Normal"�6+   �� 5`       � ��$ Center=�69   �� 5a       � ��$ 30��6'   �� 5b       � |� . 6*   �� 5c       �@��
IsReadOnly .  6+   5d      �� 5f   
   " ��6)   .�5g      ��  � ��$
 Left=�64   �� 5h       � ��$ Center=�62   �� 5i       � ��$ 5,4,5,5q�6(   �� 5j       � ��$ 1q�60   �� 5k       � ��
 ��ح�6,   �� 5l       � ��$ Normal"�6+   �� 5m       � ��$ Center=�69   �� 5n       � ��$ 25��6'   �� 5o       � |� . 6*   �� 5p       �@��
IsReadOnly .  6+   �� 5q       �@��
IsEditable .  6+   5r      �� 5t   
   " ��6+   .�5u      ��  �@��Orientation$ 
Horizontal=�6,   �� 5v       � ��$ 0,0,10,0q�6'   �� 5w       � ��$ 40��6'   5x      �� 5y   
   .@�5z      m� $��MoveUpMenuItem��6-   -   5        $��Move Item Up��5z      $ Up��6C    5{      �� 5|      $��Images/arrow_Up_16xLG.png��6   5~      m� 5      $��MoveDownMenuItem��6/   -   5        $��Move Item Down��5      $
 Down��6G    5�      �� 5�      $!��Images/arrow_Down_16xLG.png��6   5�      m� 5�      $��EditMenuItem��6%   -   5        $
��Edit��5�       5�      �� 5�      $ ��Images/EditLabel_13239.png��6   5�      m� 5�      $��DeleteMenuItem��6'   -   5        $��Delete��5�       5�      �� 5�      $��Images/delete_16x.ico��6   5�      5�      5�      .��5�      �  5�   
   ��%W   '��       �� 5�      " ��6   .�5�      ��  � ��$ 0,0,0,5q�6+   5�      5�      .+�5�   
   ]� -   5        $��mainUI��5�      .+�5�      o� $	��Top=�6   .@�5�      m� $��_File��6   .@�5�      m� -   5        $��NewFileMenuItem��5�      $
��_New��66    � ToolTip$% Create a new data template file��6b    5�      �� 5�      $��Images/NewFile_6276.png��6$   5�      m� 5�      -   5        $��OpenFileMenuItem��5�      $��_Open...��67   $) #Open an existing data template file��6h    5�      �� 5�      $"��Images/folder_Open_16xLG.png��6$   5�      m� 5�      -	   5        $��ConvertFileMenuItem��5�      $��_Convert��6:   $C =Convert an old code template file to a new data template file��6z    5�      �� 5�      $��Images/ConvertFile.png��6$   5�      �� 5�      m� 5�      -
   5        $��ExitFileMenuItem��5�      $��_Exit��67    5�      �� 5�      $��Images/Close_16XLG.png��6$   5�      5�      m� 5�      $��_Help��6   .@�5�      m� -   5        $��AboutMenuItem��5�      $��_About��64   $0 *Information about this application version��6`   5�      5�      5�      � 5�      $	��Top=�6   $��10,5,0,0q�6(    � ColumnDefinitions 5�      �� 5�      -   5         �Name$ DisplayGrid��5�   '   $��1019* �6:   �� 5�      -   5        $ 
EditButton��5�   '   $	��165 �69    � RowDefinitions 5�      �� 5�      -   5        $ 
EditScheme��5�   $   $��2* �66   $	��225��6B   �� 5�      -   5        $ MenuPreview��5�   $   $��1* �67   $	��190��6D   .+�5�      )  #Microsoft.Windows.Controls.DataGrid  -   5        $��TemplateDataGrid��5�   8  $��0��6T      	RowHeight$ 25Q�6�      SelectionMode$ Single=�6     CanUserReorderColumns .     CanUserAddRows .     CanUserDeleteRows .     AutoGenerateColumns .  ��6�  ��  5�        DataGridCheckBoxStyle��%�   &	       7���� 5�      " ��6>   8   .�5�      ��  � ��$ Center=�6B   �� 5�       � ��$ 8,0,3,0q�67   5�      5�         Columns 5�      3  -Microsoft.Windows.Controls.DataGridTextColumn  5�      /  )Microsoft.Windows.Controls.DataGridColumn   Header$ 
Sort Order��65      CanUserSort .     
IsReadOnly . 4  .Microsoft.Windows.Controls.DataGridBoundColumn    Binding  6I   �� ! �� Path$! 	SortOrder�" �� 	Converter#" [ 6�     5�      #   MinWidth$# 40Q�65   $
 Type��6C    .    6Q   �� $
! Type�#" [ $   ElementStyle$ 5�      �� 5�   "   " ��6(   % �� Triggers% 5�   &   �� 5�   *   & �� Binding& 66   �� *Type+' �� Value' Note6O   .a�5�   .   ��  �@�ToolTip# [ 6H   5�   +   �� 5�   *   & 66   �� *Type+' Counter6O   .a�5�   .   ��  �@�ToolTip# [ 6H   5�   +   �� 5�   *   & 66   �� *Type+' FixedChoice6O   .a�5�   .   ��  �@�ToolTip# [	 6H   5�   +   �� 5�   *   & 66   �� *Type+' File6O   .a�5�   .   ��  �@�ToolTip# [
 6H   5�   +   �� 5�   *   & 66   �� *Type+
' Folder6O   .a�5�   .   ��  �@�ToolTip# [ 6H   5�   +   �� 5�   *   & 66   �� *Type+' Date6O   .a�5�   .   ��  �@�ToolTip# [ 6H   5�   +   �� 5�   *   & 66   �� *Type+' Time6O   .a�5�   .   ��  �@�ToolTip# [ 6H   5�   +   �� 5�   *   & 66   �� *Type+' ImageQuality6O   .a�5�   .   ��  �@�ToolTip# [ 6H   5�   +   5�   #   5�        5�      $# 40Q�65   $ Default Value��6D    .    6[   �� $! DefaultValue�#" [ $ 5�      �� 5�   "   " ��6(   .�5�   &   ��  �@�ToolTip# [  6@   % 5�   &   �� 5�   *   & 66   �� *Type+' Counter6O   .a�5�   .   ��  �@��TextAlignment$ Right=�6N   5�   +   5�   #   5�        5�      $# 40Q�65   $ Label��6D    .    6S   �� $! Label�#" [ $ 5�      �� 5�   "   " ��6(   .�5�   &   ��  �@�ToolTip# [ 6@   5   #   5       5     $# 40Q�65   $ 
Data Label��6D    .    6X   �� $! 	DataLabel�#" [ $ 5     �� 5  "   " ��6(   .�5  &   ��  �@�ToolTip# [ 6@   5  #   5	       5
     $# 50Q�65   $ Tooltip��6D   (   Width8  2Microsoft.Windows.Controls.DataGridLengthConverter$( * 6�    .    6U   �� $! Tooltip�#" [ $ 5     �� 5  "   " ��6(   .�5  &   ��  �@�ToolTip# [ 6@   5  #   5       5     $# 40Q�65   $ Width��6D    .    6S   �� $! TXTBOXWIDTH�#" [ $ 5     �� 5  "   " ��6(   .�5  &   ��  �@�ToolTip# [ 6@   5  #   5     7  1Microsoft.Windows.Controls.DataGridTemplateColumn  5     $# 25Q�6:   $
( Auto 6H   $ Copyable��6U   )   SortMemberPath$) Include��6g    .  *   CellTemplate* 5     �� 5  "   .R�5  &   �� $��CheckBox��6/   $��Center=�6?   $��Center=�6\   + d� 	IsChecked+ 6w   �� $! Copyable�, �� UpdateSourceTrigger$, PropertyChanged=�5  #   -   	CellStyle- 5     �� 5  "   -	  'Microsoft.Windows.Controls.DataGridCell" 	 6(   .�5   &   ��  �@�ToolTip# [ 6@   5!  #   5#       5$     $# 25Q�69   $
( Auto 6G   $ Visible��6T   $) Include��6e    .  * 5%     �� 5&  "   .R�5'  &   �� $��CheckBox��6/   $��Center=�6?   $��Center=�6\   + 6w   �� $! Visible�$, PropertyChanged=�5(  #   - 5*     �� 5+  "   " 	 6(   .�5,  &   ��  �@�ToolTip# [ 6@   5-  #   5/       50     $# 40Q�69   $( 80 6G   $
 List��6R   $) Include��6`    .  * 51     �� 52  "   .R�53  &   �� .+�54  *   �� $��comboBox��63   -   5        . �� 
IsEditable. . / �� IsTextSearchEnabled/ .   54  U   �� $! 	SortOrder���6t   �� $
! List�0 �� Mode$0 TwoWay=�#" [ ��55  .   �� 56  2   " ��68   % 57  6   �� 59  :   & 6F   �� *Type+' File6_   .a�5:  >   ��  � y�$ 	Collapsed=�6[   5;  ;   �� 5<  :   & 6F   �� *Type+
' Folder6_   .a�5=  >   ��  � y�$ 	Collapsed=�6[   5>  ;   �� 5?  :   & 6F   �� *Type+' Date6_   .a�5@  >   ��  � y�$ 	Collapsed=�6[   5A  ;   �� 5B  :   & 6F   �� *Type+' Time6_   .a�5C  >   ��  � y�$ 	Collapsed=�6[   5D  ;   �� 5E  :   & 6F   �� *Type+' Counter6_   .a�5F  >   ��  � y�$ 	Collapsed=�6[   5G  ;   �� 5H  :   & 6F   �� *Type+' Note6_   .a�5I  >   ��  � y�$ 	Collapsed=�6[   5J  ;   �� 5K  :   & 6F   �� *Type+' FixedChoice6_   .a�5L  >   ��  �@�ToolTip# [ 6X   5M  ;   5O  3   ��5Q  .   �� 5R  2   " ��68   .�5S  6   ��  �@�ContextMenu# [ 6T   % 5U  6   �� 5W  :   & 6F   �� ' 
[NEW ITEM]6Z   .a�5X  >   ��  � ��$ Italic$�6Z   5Y  ;   5[  3   5]  +   5^  '   5_  #   5a     5c     ]� 5e     $��0��6   $��1��6)   $��Stretch=�69   $��Stretch=�6U   $
��10,0q�6s   .+�5f     �� $��Edit Data Items��6   $	��Top=�66   $��Center=�6K   $��18(�6h   �� 5g     -   5        $��MoveRowUpButton��5g  y   $��Move Item Up��6   $	��Top=�64   {�.  $��Stretch=�6[   $��30��6�   $ Up��6�   6�   �� 5h     -   5        $��MoveRowDownButton��5h  {   $��Move Item Down��6   $	��Top=�66   {�.  $��Stretch=�6]   $��30��6�   $
 Down��6�   6�   �� 5i     $	��Top=�6   
���   �$��1q�6F   $��0,10,0,5q�6Z   ._�5j     ]� $	��Top=�6$   .+�5k     �� $	��Add��6$   $	��Top=�62   $��Center=�6G   $��14(�6d   A� 5l     $��0,0,0,5q�6*   1 A� Rows$1 1��6;   $��Stretch=�6D   $	��Top=�6b   .+�5m  "   �� -   5        $��AddCountRowButton��5m  �   $��Count��6)   $��	5,0,2.5,0q�6:   $ Count��6M   $	��Top=�6Y   $��Stretch=�6n   $��30��6�   {�.  6�   �� 5n  "   -   5        $��AddChoiceRowButton��5n  �   $��Choice��6)   $��2.5,0q�6:   $ Choice��6I   $	��Top=�6V   $��Stretch=�6k   $��30��6�   {�.  6�   �� 5o  "   -   5        $��AddNoteRowButton��5o  �   $
��Note��6)   $��	2.5,0,5,0q�68   $
 Note��6L   $	��Top=�6W   $��Stretch=�6l   $��30��6�   {�.  6�   5p     5q     5r     �� 5s     -   5        $��RemoveRowButton��5s  i   $��Remove Item��6   $��Bottom=�63   $��Stretch=�6K   {�.  $��30��6�   $��Bottom=�6�   $��0q�6�   5u     �� 5w     $��1��6   $��2��6,   $��2��5x     
�������$��
-10,10,0,0q�63   $	��Top=�5y     $��Stretch=�5z     ]� 5|     $��1��6   $��2��6)   $��0,7,0,0q�6=   .+�5}     �� $	��Top=�6    $��	0,10,10,0q�65   $
��Bold"�6H   $|��vWhat the interface will (roughly) look like. Change the window size to see how things will rearrange themselves to fit��6Z   6�   �� 5~     
�������
�������$��1q�6C   $	��Top=�6W   $��	0,0,10,10q�6l   ._�5     �� .��5�     � -   5        2 �Name$2 wp��5�  (   $��
10,10,10,0q�64   3 � Orientation$3 
Horizontal=�6H   5�     5�     5�     5�     5�     5�     5�     