   M S B A M L   `   `   `  ���� K  GVolumecontroller, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:VolumecontrollerVolumecontroller  !    Volumecontroller.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:Volumecontroller     -     � Title$  Volume Controller��$	��164��$	��331�� � 
ResizeMode$ CanMinimize=� � WindowStartupLocation$ CenterScreen=� � Icon$ speaker.ico��.��� $��Center=�$��10,4,4,9q�$	��311�� � RowDefinitions �� $	��35* ��� $	��87* � � ColumnDefinitions �� $	��73* �.+��� -    ��Name$ status_text��$
��Left=�$��	34,-4,0,0q�$
��Wrap=�$	��Top=�$	��239��$��status�� �� TextAlignment$ Center=�$��24(�$��32���� -    ��Name$ 
ConnectBtn��$��Connect��$
��Left=�$��
111,10,0,0q�$	��Top=�$	��162��$��38��$��1��$��16(�$��Malgun Gothic)��� -   $ 	textBlock��$
��Left=�$��
172,71,0,0q�$
��Wrap=�$	��Top=�$��16��$��1��$��1���� -   $ 	enter_new��$
��Left=�$��	31,23,0,0q�$
��Wrap=�$	��Top=�$��17��$	��118��$��	COM port:��$��2���� -   	 ��Name$	 COM_text_box��$
��Left=�$��38��$��	31,10,0,0q�
 �� TextWrapping$

 Wrap=�$	��Top=�$��75��$��1��$��9��$��24(� �� TextAlignment$ Center=��� -    ��Name$ checkBox��$��Run at startup��$
��Left=�$��	31,56,0,0q�$��1��$	��Top=�$��21��$��95��