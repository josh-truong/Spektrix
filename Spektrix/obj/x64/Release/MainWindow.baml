   M S B A M L   `   `   `  ���� C  ?Spektrix, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:SpektrixSpektrix  9!clr-namespace:Spektrix.ComponentsSpektrix.Components  3clr-namespace:Spektrix.WidgetsSpektrix.Widgets      Spektrix.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:Spektrix     8
components!clr-namespace:Spektrix.Components     2widgetsclr-namespace:Spektrix.Widgets     -     � WindowStyle$
  None=� � WindowState$ Normal=� � WindowStartupLocation$ Manual=�
 � Top$ 0�� � Left$ 0�� � AllowsTransparency .  � Topmost .  � ShowInTaskbar .  � Title$ 
MainWindow��#��Z��#��Z��.��� -   	 �Name$	 MainGrid��
 � RowDefinitions
 �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� ��  � ColumnDefinitions �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� .+�� -   $	 exit_background��
������ $��0��$��20��$��0��$��21��� $��1��$��3��$��6��$��9��.+�)   #Spektrix.Components.DateTimeDisplay  -     Name$ main_display_clock��� -   $	 startup_bar��$��0��$��2��$��17��.+��� -   
���  ��$��0,0,0,0q�$��1��$��Stretch=�$	��Top=�� $��3��$��3��$��1��$��19��
 �� �� �� �� .+�!   Spektrix.Components.TabMenu  -     Name$ main_tab_bar��$��1��$��2��   Spektrix.Components.Group  -     Name$ 
main_group��$��6��$��8��$��1��$��19��� -	   $	 menu_widget_hub��$��14��$��5��$��1��$��19��
 �� �� �� �� �� �� .+�� $��0��
 �� �� �� .+��� -
    ��Name$ 
bottom_bar��
�������$
��Left=����� *ActualWidth+ �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType" �� $��1��$��5��
 �� �� �� �� �� �� �� �� �� ��  �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� .+�!   Spektrix.Widgets.MultiMedia  -     Name$ main_multimedia��$��1��$��9��$��12��$��4��   Spektrix.Widgets.Calandar  -     Name$ main_calandar��$��1��$��9��$��0��$��3��$   Spektrix.Widgets.CpuRamMonitor  -     Name$ main_cpu_ram_monitor��$��1��$��4��$��16��$��4��"   Spektrix.Widgets.DiskMonitor  -     Name$ main_disk_monitor��$��5��$��5��$��16��$��4��%   Spektrix.Widgets.NetworkMonitor  -     Name$ main_network_monitor��$��1��$��9��$��20��$��4��$��0q�