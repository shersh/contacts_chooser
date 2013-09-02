contacts_chooser
================

Contact chooser for Windows Phone platform. (Like control in native email application)


This control is fully customizable via template bindings and styling. 

Usage:

<controls2:ContactsChooser x:Name="chooser"
                                   Grid.Row="1"
                                   Margin="0,24,0,0"
                                   HeaderText="Partipants"
                                   ItemsSource="{Binding InvitedUsers}"
                                   PopupBackground="Transparent"
                                   PopupBorderBrush="Transparent"
                                   SearchCommand="{Binding SearchCommand}"
                                   SearchItemsSource="{Binding SearchedUsers}"/>
                                   
                                   
Where ItemsSource is already selected contacts, SearchItemsSource is collection that used when serching command is compled for showing available contacts for selecting.
SearchCommand is ICommand for searching implementaion. You can use it for searching via web-service for example.

See Sample project for more information.

Known Issue:

DataContext for this control should be set when Page.Loaded event fired. If it's problem I'll fix it. 
