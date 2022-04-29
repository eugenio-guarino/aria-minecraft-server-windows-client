using System.Windows.Forms;
using System.Drawing;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Form1 : Form
{
    HttpClient httpClient = new HttpClient();

    public void FormLayout()
    {
        this.Name = "Aria Server Launcher";
        this.Text = "Aria Server Launcher";
        this.Size = new System.Drawing.Size(350, 400);

        // avoid resizing
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        

        // create snarkyPhraseLabel to
        Label titleLabel = new Label();
        titleLabel.Text = "ARIA";
        titleLabel.Location = new Point(70, 10);
        titleLabel.Font = new Font("Wide Latin", 12);
        titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        titleLabel.Size = new Size(200, 60);
        this.Controls.Add(titleLabel);
        this.ActiveControl = titleLabel;

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
        tokenTextBox.Name = "tokenTextBox";
        tokenTextBox.Location = new Point(80, 175);
        tokenTextBox.Size = new Size(180, 50);
        tokenTextBox.Font = new Font("Consolas", 9);
        this.Controls.Add(tokenTextBox);

        //create button to start the server
        Button startButton = new Button();
        startButton.Text = "AVVIA IL SERVER";
        startButton.Location = new Point(70, 235);
        startButton.Font = new Font("Tahoma", 11, FontStyle.Bold);
        startButton.Size = new Size(200, 50);
        startButton.BackColor = Color.SkyBlue;
        startButton.UseCompatibleTextRendering = true;
        this.Controls.Add(startButton);

        Label statusLabel = new Label();
        statusLabel.Name = "statusLabel";
        statusLabel.Location = new Point(70, 290);
        statusLabel.Size = new Size(200, 50);
        statusLabel.Font = new Font("Consolas", 9);
        statusLabel.TextAlign = ContentAlignment.MiddleCenter;
        this.Controls.Add(statusLabel);

        // click handler
        startButton.Click += new EventHandler(this.StartServer);
    }

    async void StartServer(object sender, EventArgs e)
    {
        Label statusLabel = this.Controls.Find("statusLabel" , true).FirstOrDefault() as Label;
        TextBox tokenBox = this.Controls.Find("tokenTextBox" , true).FirstOrDefault() as TextBox;
        string githubToken = tokenBox.Text;
        string username = "";
        string repo = "";
        string eventType = "";

        if (String.IsNullOrEmpty(githubToken)){
            statusLabel.Text = "Code can't be empty.";
            statusLabel.ForeColor = Color.DarkRed;
            return;
        }

        var content = new FormUrlEncodedContent(new[]{new KeyValuePair<string, string>("event_type", eventType)});

        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", githubToken); 

        var httpResponseMessage = await httpClient.PostAsync(new Uri($"https://api.github.com/repos/{username}/{repo}/dispatches"), content);

        if (httpResponseMessage.ToString().Contains("StatusCode: 204")){
            statusLabel.Text = "Operation successful.";
            statusLabel.ForeColor = Color.DarkGreen;
        }
        else{
            statusLabel.Text = "Something went wrong.";
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
