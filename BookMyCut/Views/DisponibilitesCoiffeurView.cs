<Window x:Class="BookMyCut.Views.DisponibilitesCoiffeurView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             mc:Ignorable="d"
             d:DesignHeight="650"
             d:DesignWidth="1100"
             Background="Transparent">

    <Grid Margin="40">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>

        <StackPanel Margin="0,0,0,25">
            <TextBlock Text="Mes disponibilités"
                       FontSize="30"
                       FontWeight="Bold"
                       Foreground="#E0B03D"
                       HorizontalAlignment="Center"/>

            <TextBlock Text="Définissez vos heures de travail pour permettre aux clients de réserver selon vos créneaux disponibles."
                       FontSize="15"
                       Foreground="#DDDDDD"
                       HorizontalAlignment="Center"
                       Margin="0,8,0,0"
                       TextWrapping="Wrap"
                       TextAlignment="Center"/>
        </StackPanel>

        <Grid Grid.Row="1">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="320"/>
                <ColumnDefinition Width="20"/>
                <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>

            <!-- Formulaire -->
            <Border Grid.Column="0"
                    Background="#33000000"
                    CornerRadius="18"
                    Padding="25">
                <StackPanel>
                    <TextBlock Text="Créer une disponibilité"
                               FontSize="20"
                               FontWeight="Bold"
                               Foreground="White"
                               Margin="0,0,0,20"/>

                    <TextBlock Text="Date"
                               Foreground="White"
                               Margin="0,0,0,6"/>
                    <DatePicker SelectedDate="{Binding DateSelectionnee}"
                                Margin="0,0,0,18"/>

                    <TextBlock Text="Heure de début (HH:mm)"
                               Foreground="White"
                               Margin="0,0,0,6"/>
                    <TextBox Text="{Binding HeureDebut, UpdateSourceTrigger=PropertyChanged}"
                             Padding="10"
                             Margin="0,0,0,18"/>

                    <TextBlock Text="Heure de fin (HH:mm)"
                               Foreground="White"
                               Margin="0,0,0,6"/>
                    <TextBox Text="{Binding HeureFin, UpdateSourceTrigger=PropertyChanged}"
                             Padding="10"
                             Margin="0,0,0,25"/>

                    <Button Content="CRÉER LES CRÉNEAUX"
                            Command="{Binding CreerDisponibiliteCommand}"
                            Height="45"
                            Background="#E86C1A"
                            Foreground="White"
                            FontWeight="Bold"
                            BorderThickness="0"
                            Cursor="Hand">
                        <Button.Resources>
                            <Style TargetType="Border">
                                <Setter Property="CornerRadius" Value="8"/>
                            </Style>
                        </Button.Resources>
                    </Button>

                    <Border Background="#22FFFFFF"
                            CornerRadius="10"
                            Padding="12"
                            Margin="0,20,0,0">
                        <TextBlock Text="Exemple : si vous entrez 09:00 à 12:00, le système peut créer automatiquement plusieurs créneaux de 30 minutes."
                                   Foreground="#DDDDDD"
                                   TextWrapping="Wrap"
                                   FontSize="12"/>
                    </Border>
                </StackPanel>
            </Border>

            <!-- Liste -->
            <Border Grid.Column="2"
                    Background="#33000000"
                    CornerRadius="18"
                    Padding="20">
                <Grid>
                    <Grid.RowDefinitions>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="*"/>
                        <RowDefinition Height="Auto"/>
                    </Grid.RowDefinitions>

                    <TextBlock Text="Mes créneaux enregistrés"
                               FontSize="20"
                               FontWeight="Bold"
                               Foreground="White"
                               Margin="0,0,0,15"/>

                    <Border Grid.Row="1"
                            Background="#11FFFFFF"
                            CornerRadius="10"
                            Padding="5">
                        <DataGrid ItemsSource="{Binding MesDisponibilites}"
                                  SelectedItem="{Binding DisponibiliteSelectionnee}"
                                  AutoGenerateColumns="False"
                                  IsReadOnly="True"
                                  Background="Transparent"
                                  Foreground="Black"
                                  BorderThickness="0"
                                  RowHeight="38">
                            <DataGrid.Columns>
                                <DataGridTextColumn Header="Début"
                                                    Binding="{Binding Debut, StringFormat={}{0:dd/MM/yyyy HH:mm}}"
                                                    Width="2*"/>

                                <DataGridTextColumn Header="Fin"
                                                    Binding="{Binding Fin, StringFormat={}{0:HH:mm}}"
                                                    Width="*"/>

                                <DataGridCheckBoxColumn Header="Réservé"
                                                        Binding="{Binding EstReserve}"
                                                        Width="*"/>
                            </DataGrid.Columns>
                        </DataGrid>
                    </Border>

                    <Button Grid.Row="2"
                            Content="SUPPRIMER LE CRÉNEAU SÉLECTIONNÉ"
                            Command="{Binding SupprimerDisponibiliteCommand}"
                            Height="45"
                            Margin="0,18,0,0"
                            Background="#E74C3C"
                            Foreground="White"
                            FontWeight="Bold"
                            BorderThickness="0"
                            Cursor="Hand">
                        <Button.Resources>
                            <Style TargetType="Border">
                                <Setter Property="CornerRadius" Value="8"/>
                            </Style>
                        </Button.Resources>
                    </Button>
                </Grid>
            </Border>
        </Grid>
    </Grid>
</Window>
