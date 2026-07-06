$file = 'E:\client\AMS\Account-Management\Updated-code\AMS\Views\MainWindow.xaml'
$content = Get-Content $file -Raw

$content = $content -replace '(?s)\s*<!-- LEFT SIDEBAR -->.*?<!-- CONTENT PANELS -->\s*<Border[^>]+>\s*<Grid>', "
            <!-- LEFT SIDEBAR & CONTENT -->
            <TabControl Grid.Column="1" TabStripPlacement="Left" Margin="0">"

$content = $content -replace '(?s)                    <!-- WELCOME PAGE -->\s*<Grid Visibility="\{Binding CurrentPage, Converter=\{StaticResource NullToVis\}\}"\s*x:Name="PageWelcome">\s*<Grid\.Style>.*?</Grid\.Style>', "                    <!-- WELCOME PAGE -->
                    <TabItem Header="Welcome">"
$content = $content -replace '(?s)(TxtDbStatus".*?Visibility="Collapsed"/>\s*</StackPanel>)\s*</Grid>', "$1
                    </TabItem>"

$content = $content -replace '(?s)                    <!-- ACCOUNTS PAGE -->\s*<Grid x:Name="PageAccounts">\s*<Grid\.Style>.*?</Grid\.Style>', "                    <!-- ACCOUNTS PAGE -->
                    <TabItem Header="Accounts">"
$content = $content -replace '(?s)                                        <!-- Account list -->.*?</Grid>\s*</Grid>\s*</DockPanel>\s*</Grid>', "$0" -replace '(?s)                    <!-- STOCKS PAGE -->', "
                    </TabItem>
                    <!-- STOCKS PAGE -->"

$content = $content -replace '(?s)                    <Grid x:Name="PageStocks">\s*<Grid\.Style>.*?</Grid\.Style>', "                    <TabItem Header="Manage Stocks">"
$content = $content -replace '(?s)                    <!-- CUSTOMERS PAGE -->', "
                    </TabItem>
                    <!-- CUSTOMERS PAGE -->"

$content = $content -replace '(?s)                    <Grid x:Name="PageCustomers">\s*<Grid\.Style>.*?</Grid\.Style>', "                    <TabItem Header="Manage Parties">"
$content = $content -replace '(?s)                    <!-- AGENTS PAGE -->', "
                    </TabItem>
                    <!-- AGENTS PAGE -->"

$content = $content -replace '(?s)                    <Grid x:Name="PageAgents">\s*<Grid\.Style>.*?</Grid\.Style>', "                    <TabItem Header="Manage Agents">"
$content = $content -replace '(?s)                    <!-- SALES PAGE -->', "
                    </TabItem>
                    <!-- SALES PAGE -->"

$content = $content -replace '(?s)                    <Grid x:Name="PageSales">\s*<Grid\.Style>.*?</Grid\.Style>', "                    <TabItem Header="Sale Autos">"
$content = $content -replace '(?s)                    <!-- REPORTS PAGE -->', "
                    </TabItem>
                    <!-- REPORTS PAGE -->"

$content = $content -replace '(?s)                    <Grid x:Name="PageReports">\s*<Grid\.Style>.*?</Grid\.Style>', "                    <TabItem Header="Reports">"

$content = $content -replace '(?s)                        </Grid>\s*</DockPanel>\s*</Grid>\s*</Grid>\s*</Border>\s*<!-- -- STATUS BAR -- -->', "                        </Grid>
                    </DockPanel>
                    </TabItem>
            </TabControl>
        </Grid>

        <!-- -- STATUS BAR -- -->"

$content | Set-Content $file
