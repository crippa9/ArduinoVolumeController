   M S B A M L   `   `   `  ����K  GVolumecontroller, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:VolumecontrollerVolumecontroller  5   	   !    Volumecontroller.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:Volumecontroller     5   	   -   5          � Title$  Volume Controller��5   	   $	��164��5	   	   $	��331��5
   	    � 
ResizeMode$ CanMinimize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � Icon$ speaker.ico��5   	   .��5      � $��Center=�6   $��10,4,4,9q�6(   $	��311��6:    � RowDefinitions 5   
   �� 5      $	��35* �6   �� 5      $	��87* �6    � ColumnDefinitions 5   
   �� 5      $	��73* �6   .+�5   
   �� -   5         ��Name$ status_text��5      $
��Left=�6)   $��	34,-4,0,0q�6D   $
��Wrap=�6W   $	��Top=�6k   $	��239��6�   $��status��6�    �� TextAlignment$ Center=�6�   $��24(�6�   $��32��6�   �� 5   
   -   5         ��Name$ 
ConnectBtn��5      $��Connect��6%   $
��Left=�67   $��
111,10,0,0q�6R   $	��Top=�6f   $	��162��6~   $��38��6�   $��1��6�   $��16(�6�   $��Malgun Gothic)�6�   �� 5   
   -   5        $ 	textBlock��5      $
��Left=�6'   $��
172,71,0,0q�6B   $
��Wrap=�6V   $	��Top=�6j   $��16��6�   $��1��6�   $��1��6�   �� 5   
   -   5        $ 	enter_new��5      $
��Left=�6'   $��	31,23,0,0q�6B   $
��Wrap=�6U   $	��Top=�6i   $��17��6�   $	��118��6�   $��	COM port:��6�   $��2��6�   �� 5   
   -   5        	 ��Name$	 COM_text_box��5      $
��Left=�6(   $��38��6C   $��	31,10,0,0q�6O   
 �� TextWrapping$

 Wrap=�6b   $	��Top=�6v   $��75��6�   $��1��6�   $��9��6�   $��24(�6�    �� TextAlignment$ Center=�6�   �� 5   
   -   5         ��Name$ checkBox��5      $��Run at startup��6%   $
��Left=�6>   $��	31,56,0,0q�6Y   $��1��6l   $	��Top=�6y   $��21��6�   $��95��6�   6�   5!      5"      