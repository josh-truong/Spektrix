   M S B A M L   `   `   `  ���� C  ?Spektrix, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Spektrix.WidgetsSpektrix.Widgets  %    Spektrix.Widgets.NetworkMonitor   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:Spektrix.Widgets     .���� -     ��Name$  root_border��
�������$��3q� �� CornerRadius$ 10��._�� $��10q� � RowDefinitions �� �� �� �� �� �� .+�� $��0��.+�(� .���� -    ��Name$ title��
�������$��0,0,0,5q�   	Baksheesh#��[  $��Normal$�$��Center=�$��Center=� �� TextAlignment$ Center=�$��	Underline{�$��NETWORK��� -    �Name$ 	wifi_ipv4��$��1�� � ColumnDefinitions �� �� .+�(� $��0��$
��Left=�$��Center=�.���� -   $ wifi_ipv4_col0��
�������#��[  $��Normal$�$��3,2,0,2q�$ Center=�$��WI-FI IPv4:��(� $��1��$��Right=�$��Center=�.���� -   $ wifi_ipv4_col1��
�������#��[  $��Normal$�$��0,2,0,2q�$ Center=�$��___.__.__.__��� -   $ ethernet_ipv4��$��2�� �� �� .+�(� $��0��$
��Left=�$��Center=�.���� -   $ ethernet_ipv4_col0��
�������#��[  $��Normal$�$��3,2,0,2q�$ Center=�$��Ethernet IPv4:��(� $��1��$��Right=�$��Center=�.���� -   $ ethernet_ipv4_col1��
�������#��[  $��Normal$�$��0,2,0,2q�$ Center=�$��___.__.__.__��� $��3�� �� �� �� .+�(� $��0��$
��Left=�$��Center=�$��3q�.���� -	   $ 
speed_col0��
�������#��[  $��Normal$�$ Center=�$��--- B/s��� $��1��$��2,0,2,0q� �� �� �� �� �� .+�(� $��0��$��Stretch=�$��Stretch=�$��0,0,5,0q�.���� $��Center=�$��Center=�$��UniformToFill=�����  �� 	UriSource$: 4pack://application:,,,/Resources/Assets/Received.png?�(� $��1��$��3��$��Center=�$��Center=�$��3q�.���� -
   $ 
speed_col1��
�������#��[  $��Normal$�$ Center=�$��Speed��(� $��4��$��Stretch=�$��Stretch=�$��5,0,0,0q�.���� $��Center=�$��Center=�$��UniformToFill=����� $6 0pack://application:,,,/Resources/Assets/Sent.png?�(� $��2��$��Right=�$��Center=�$��3q�.���� -   $ 
speed_col2��
�������#��[  $��Normal$�$ Center=�$��--- B/s��� $��4�� �� �� �� .+�(� $��0��$
��Left=�$��Center=�$��3q�.���� -   $ 	peak_col0��
�������#��[  $��Normal$�$ Center=�$��--- MB/s��� $��1��$��2,0,2,0q� �� �� �� �� �� .+�(� $��0��$��Stretch=�$��Stretch=�$��0,0,5,0q�.���� $��Center=�$��Center=�$��UniformToFill=�$��5q����� $: 4pack://application:,,,/Resources/Assets/Received.png?�(� $��1��$��3��$��Center=�$��Center=�$��3q�.���� -   $ 	peak_col1��
�������#��[  $��Normal$�$ Center=�$
��Peak��(� $��4��$��Stretch=�$��Stretch=�$��5,0,0,0q�.���� $��Center=�$��Center=�$��UniformToFill=�$��5q����� $6 0pack://application:,,,/Resources/Assets/Sent.png?�(� $��2��$��Right=�$��Center=�$��3q�.���� -   $ 	peak_col2��
�������#��[  $��Normal$�$ Center=�$
��kB/s��� $��5�� �� �� �� .+�(� $��0��$
��Left=�$��Center=�$��3q�.���� -   $ sum_col0��
�������#��[  $��Normal$�$ Center=�$��--- GB��� $��1��$��2,0,2,0q� �� �� �� �� �� .+�(� $��0��$��Stretch=�$��Stretch=�$��0,0,5,0q�.���� $��Center=�$��Center=�$��UniformToFill=�$��5q����� $: 4pack://application:,,,/Resources/Assets/Received.png?�(� $��1��$��3��$��Center=�$��Center=�$��3q�.���� -   $ sum_col1��
�������#��[  $��Normal$�$ Center=�$	��SUM��(� $��4��$��Stretch=�$��Stretch=�$��5,0,0,0q�.���� $��Center=�$��Center=�$��UniformToFill=�$��5q����� $6 0pack://application:,,,/Resources/Assets/Sent.png?�(� $��2��$��Right=�$��Center=�$��3q�.���� -   $ sum_col2��
�������#��[  $��Normal$�$ Center=�$��--- GB��