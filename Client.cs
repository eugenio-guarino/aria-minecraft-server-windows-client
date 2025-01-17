using System.Windows.Forms;
using System.Drawing;
using System;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

public class Client : Form
{
    public void FormLayout()
    {   
        // General settings
        this.Name = "Aria Server Launcher";
        this.Text = "Aria Server Launcher";
        this.Size = new System.Drawing.Size(350, 480);

        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // Title label
        System.Windows.Forms.Label titleLabel = new System.Windows.Forms.Label();
        titleLabel.Text = "AriA";
        titleLabel.Location = new Point(70, 10);
        titleLabel.Font = new Font("Unispace", 20); // Use the first font family in the collection        
        titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        titleLabel.Size = new Size(200, 60);
        this.Controls.Add(titleLabel);
        this.ActiveControl = titleLabel;

        // Retrieve random funny phrase and put it in the label
        string randomPhrase = RetrieveFunnyPhrase(PhrasesList.Phrases);
        System.Windows.Forms.Label snarkyPhraseLabel = new System.Windows.Forms.Label();
        snarkyPhraseLabel.Text = randomPhrase;
        snarkyPhraseLabel.Location = new Point(70, 60);
        snarkyPhraseLabel.Font = new Font("Consolas", 8);
        snarkyPhraseLabel.TextAlign = ContentAlignment.MiddleCenter;
        snarkyPhraseLabel.Size = new Size(200, 60);
        this.Controls.Add(snarkyPhraseLabel);

        // TextBox for secret code
        TextBox tokenTextBox = new TextBox();
        tokenTextBox.Text = "paste the code here"; // Set initial placeholder text
        tokenTextBox.ForeColor = SystemColors.GrayText; // Set placeholder text color
        tokenTextBox.Name = "tokenTextBox";
        tokenTextBox.Location = new Point(80, 175);
        tokenTextBox.Size = new Size(180, 75);
        tokenTextBox.Multiline = true;
        tokenTextBox.Font = new Font("Consolas", 9);
        tokenTextBox.Enter += TokenTextBox_Enter;
        tokenTextBox.Leave += TokenTextBox_Leave;

        // allows to select all text
        tokenTextBox.KeyDown += (sender, e) =>
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                tokenTextBox.SelectAll(); // Select all text in the TextBox
                e.SuppressKeyPress = true; // Prevent default sound or behavior
            }
        };
        
        this.Controls.Add(tokenTextBox);


        // Start Button
        Button startButton = new Button
        {
            Text = "START MC SERVER",
            Location = new Point(70, 275),
            Font = new Font("Tahoma", 11, FontStyle.Bold),
            Size = new Size(200, 50),
            FlatStyle = FlatStyle.Flat, // Allow custom painting
            BackColor = Color.LightBlue // Base color for the button
        };

        // Remove border for a cleaner look
        startButton.FlatAppearance.BorderSize = 0;

        // Add Paint event for convex effect
        startButton.Paint += (sender, e) =>
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                // Draw convex gradient (blue shades)
                using (LinearGradientBrush brush = new LinearGradientBrush(
                        btn.ClientRectangle,
                        Color.FromArgb(191, 219, 255), // Light blue
                        Color.FromArgb(128, 179, 255), // Darker blue
                        LinearGradientMode.Vertical)) // Vertical gradient
                {
                    e.Graphics.FillRectangle(brush, btn.ClientRectangle);
                }

                // Add border
                using (Pen pen = new Pen(Color.FromArgb(0, 128, 255), 2)) // Blue border
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, btn.Width - 1, btn.Height - 1);
                }

                // Draw text
                TextRenderer.DrawText(
                    e.Graphics,
                    btn.Text,
                    btn.Font,
                    btn.ClientRectangle,
                    Color.White, // White text for contrast
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        };

        // Timer for enabling the button after 5 seconds
        Timer enableTimer = new Timer { Interval = 5000 };
        startButton.Click += (s, e) =>
        {
            startButton.Enabled = false; 
            enableTimer.Start(); 
        };
        enableTimer.Tick += (s, e) =>
        {
            startButton.Enabled = true; 
            enableTimer.Stop();
        };

        startButton.Click += new EventHandler(this.StartServer);

        this.Controls.Add(startButton);

        // Status label
        System.Windows.Forms.Label statusLabel = new System.Windows.Forms.Label();
        statusLabel.Name = "statusLabel";
        statusLabel.Location = new Point(70, 340);
        statusLabel.Size = new Size(200, 50);
        statusLabel.Font = new Font("Consolas", 9);
        statusLabel.TextAlign = ContentAlignment.MiddleCenter;
        this.Controls.Add(statusLabel);

        // paypal button
        Button paypalButton = new Button();
        paypalButton.Text = "Donate via PayPal";
        paypalButton.Size = new Size(130, 30); // Adjust size as needed
        paypalButton.Font = new Font("Tahoma", 10);
        paypalButton.BackColor = Color.Blue;
        paypalButton.ForeColor = Color.White;
        paypalButton.FlatStyle = FlatStyle.Flat; // To remove button border
        paypalButton.FlatAppearance.BorderSize = 0; // To remove button border
        paypalButton.Location = new Point((this.ClientSize.Width - paypalButton.Width) / 2, 400); // Center horizontally


        // Event handler for PayPal button click
        paypalButton.Click += new EventHandler((sender, e) =>
        {
            // Open PayPal link
            System.Diagnostics.Process.Start("https://www.paypal.me/ungenio6");
        });

        this.Controls.Add(paypalButton);

    }

    void TokenTextBox_Enter(object sender, EventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        if (textBox.Text == "paste the code here") // Correct comparison
        {
            textBox.Text = ""; // Clear placeholder text
            textBox.ForeColor = SystemColors.WindowText; // Restore default text color
        }
    }

    // Event handler for when the TextBox loses focus
    void TokenTextBox_Leave(object sender, EventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        if (string.IsNullOrWhiteSpace(textBox.Text)) // Check for empty input
        {
            textBox.Text = "paste the code here"; // Reset placeholder text
            textBox.ForeColor = SystemColors.GrayText; // Set placeholder text color
        }
    }

    void StartServer(object sender, EventArgs ev)
    {
        System.Windows.Forms.Label statusLabel = this.Controls.Find("statusLabel", true).FirstOrDefault() as System.Windows.Forms.Label;
        TextBox tokenBox = this.Controls.Find("tokenTextBox", true).FirstOrDefault() as TextBox;
        string githubToken = tokenBox.Text.Replace(" ", ""); // Removing spaces from the token
        string username = "eugenio-guarino";
        string repo = "aria-minecraft-server-iac";
        string result = " ";

        if (String.IsNullOrEmpty(githubToken))
        {
            statusLabel.Text = "Code can't be empty.";
            statusLabel.ForeColor = Color.DarkRed;
            return;
        }

        // Couldn't get the http request to work on HTTP Client, so CMD process was used instead.
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.UseShellExecute = false;
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = $@"/C curl --request POST --url ""https://api.github.com/repos/{username}/{repo}/dispatches"" --header ""authorization: Bearer {githubToken}""" + @" --data ""{\""event_type\"":\""create-infr\""}""";
        process.StartInfo = startInfo;

        process.OutputDataReceived += (s, e) => result += e.Data;
        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();


        if (!(result.IndexOf("bad credentials", StringComparison.OrdinalIgnoreCase) >= 0))
        {
            statusLabel.Text = "Success! World will be ready in 5 minutes.";
            statusLabel.ForeColor = Color.DarkGreen;
        }
        else
        {
            statusLabel.Text = "Wrong secret code.";
            statusLabel.ForeColor = Color.DarkRed;
        }
    }

    string RetrieveFunnyPhrase(string[] phrases)
    {
        string chosen = null;
        int numberSeen = 0;
        var rng = new Random();
        foreach (string line in phrases)
        {
            if (rng.Next(++numberSeen) == 0)
            {
                chosen = line;
            }
        }
        return chosen;
    }
}
