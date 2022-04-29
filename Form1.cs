using System.Windows.Forms;
using System.Drawing;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;


public class Form1 : Form
{
    HttpClient httpClient = new HttpClient();

    public void FormLayout()
    {
        this.Name = "Aria";
        this.Text = "Aria";
        this.Size = new System.Drawing.Size(350, 400);

        // avoid resizing
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // create snarkyPhraseLabel to
        Label titleLabel = new Label();
        titleLabel.Text = "Aria Server Launcher";
        titleLabel.Location = new Point(70, 10);
        titleLabel.Font = new Font("Consolas", 12);
        titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        titleLabel.Size = new Size(200, 60);
        this.Controls.Add(titleLabel);

        // get a random phrase from database
        string randomPhrase = RandomPhrase(PhrasesList.Phrases);

        // create snarkyPhraseLabel to
        Label snarkyPhraseLabel = new Label();
        snarkyPhraseLabel.Text = randomPhrase;
        snarkyPhraseLabel.Location = new Point(70, 60);
        snarkyPhraseLabel.Font = new Font("Consolas", 8);
        snarkyPhraseLabel.TextAlign = ContentAlignment.MiddleCenter;
        snarkyPhraseLabel.Size = new Size(200, 60);
        this.Controls.Add(snarkyPhraseLabel);

        TextBox tokenTextBox = new TextBox();
        tokenTextBox.Text = "inserisci il codice";
        tokenTextBox.Location = new Point(80, 175);
        tokenTextBox.Size = new Size(180, 50);
        tokenTextBox.Font = new Font("Consolas", 10);
        this.Controls.Add(tokenTextBox);

        //create button to start the server
        Button startButton = new Button();
        startButton.Text = "AVVIA IL SERVER";
        startButton.Location = new Point(70, 235);
        startButton.Font = new Font("Tahoma", 13);
        startButton.Size = new Size(200, 50);
        startButton.BackColor = Color.DarkOrange;
        startButton.ForeColor= Color.White;
        startButton.UseCompatibleTextRendering = true;
        this.Controls.Add(startButton);

        Label statusLabel = new Label();
        statusLabel.Location = new Point(70, 290);
        statusLabel.Text = "lol";
        statusLabel.Font = new Font("Consolas", 9);
        this.Controls.Add(statusLabel);

        // click handler
        startButton.Click += new EventHandler(this.StartServer);
    }

    async void StartServer(object sender, EventArgs e)
    {
        Label statusLabel = this.Controls.Find("statusLabel" , true).FirstOrDefault() as Label;
        TextBox tokenBox = this.Controls.Find("tokenTextBox" , true).FirstOrDefault() as TextBox;

        try
        {
            string githubToken = tokenBox.Text;
            string username = "";
            string repo = "";
            string eventType = "";

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("event_type", eventType),
            });

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", githubToken); 

            var httpResponseMessage = await httpClient.PostAsync(new Uri($"https://api.github.com/repos/{username}/{repo}/dispatches"), content);
            string resp = await httpResponseMessage.Content.ReadAsStringAsync();

            statusLabel.Text = "Operation successful.";
            statusLabel.ForeColor = Color.DarkGreen;

        }
        catch(Exception ex)
        {
            statusLabel.Text = $"An error has occurred.";
            statusLabel.ForeColor = Color.DarkRed;
        }
        
    }

    public string RandomPhrase( string[] phrases )
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
