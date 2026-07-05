import re

with open(r"E:\client\AMS\Account-Management\Updated-code\AMS\Views\MainWindow.xaml", "r", encoding="utf-8") as f:
    content = f.read()

# Replace sidebar and wrappers with TabControl
content = re.sub(
    r'(?s)            <!-- LEFT SIDEBAR -->.*?<!-- CONTENT PANELS -->\s*<Border Grid\.Column="1"[^>]*>\s*<Grid>',
    r'            <!-- -- MAIN CONTENT AREA -- -->\n            <TabControl Grid.Row="1" TabStripPlacement="Left" Margin="0">',
    content
)

# Welcome Page
content = re.sub(
    r'(?s)                    <!-- WELCOME PAGE -->\s*<Grid Visibility="\{Binding CurrentPage[^}]+\}\}"\s*x:Name="PageWelcome">\s*<Grid\.Style>.*?</Grid\.Style>',
    r'                    <!-- WELCOME PAGE -->\n                    <TabItem Header="Welcome">\n                        <Grid x:Name="PageWelcome">',
    content
)
content = re.sub(
    r'(?s)(TxtDbStatus".*?Visibility="Collapsed"/>\s*</StackPanel>)\s*</Grid>',
    r'\1\n                        </Grid>\n                    </TabItem>',
    content
)

# Accounts Page
content = re.sub(
    r'(?s)                    <!-- ACCOUNTS PAGE -->\s*<Grid x:Name="PageAccounts">\s*<Grid\.Style>.*?</Grid\.Style>',
    r'                    <!-- ACCOUNTS PAGE -->\n                    <TabItem Header="Accounts">\n                        <Grid x:Name="PageAccounts">',
    content
)
content = re.sub(
    r'(?s)(<!-- Account list -->.*?</Grid>\s*</Grid>\s*</DockPanel>)\s*</Grid>',
    r'\1\n                        </Grid>\n                    </TabItem>',
    content
)

# Other pages
pages = [
    ("PageStocks", "Manage Stocks", "STOCKS"),
    ("PageCustomers", "Manage Parties", "CUSTOMERS"),
    ("PageAgents", "Manage Agents", "AGENTS"),
    ("PageSales", "Sale Autos", "SALES"),
    ("PageReports", "Reports", "REPORTS")
]

for name, header, uppercase in pages:
    # Open tab item
    content = re.sub(
        rf'(?s)                    <!-- {uppercase} PAGE -->\s*<Grid x:Name="{name}">\s*<Grid\.Style>.*?</Grid\.Style>',
        f'                    <!-- {uppercase} PAGE -->\n                    <TabItem Header="{header}">\n                        <Grid x:Name="{name}">',
        content
    )

content = re.sub(
    r'(?s)(DataGrid TextBlock.*?Margin="0,0,0,10"\s*/>\s*</Grid>)\s*</Grid>',
    r'\1\n                        </Grid>\n                    </TabItem>',
    content
)

# Explicitly replace the end structure if anything was left.
content = re.sub(
    r'(?s)                    <!-- REPORTS PAGE -->.*?Binding="\{Binding Total\}"\s*Width="100"/>\s*</DataGrid\.Columns>\s*</DataGrid>\s*</Grid>\s*</Grid>\s*</Border>\s*<!-- -- STATUS BAR -- -->',
    r'                        </Grid>\n                    </TabItem>\n            </TabControl>\n        <!-- -- STATUS BAR -- -->',
    content
)

with open(r"E:\client\AMS\Account-Management\Updated-code\AMS\Views\MainWindow.xaml", "w", encoding="utf-8") as f:
    f.write(content)
