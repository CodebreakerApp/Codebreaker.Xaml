namespace Codebreaker.WinForms;

/// <summary>
/// Main form for the Codebreaker game.
/// </summary>
public partial class MainForm : Form
{
    private readonly GamePageViewModel _viewModel;
    private readonly IInfoBarService _infoBarService;

    // UI Controls
    private Panel _startGamePanel = default!;
    private TextBox _usernameTextBox = default!;
    private Button _startGameButton = default!;
    
    private Panel _pegSelectionPanel = default!;
    private ComboBox[] _colorComboBoxes = default!;
    private Button _setMoveButton = default!;
    
    private ListBox _movesListBox = default!;
    private Label _statusLabel = default!;
    private ProgressBar _progressBar = default!;
    private TextBox _infoBarTextBox = default!;

    public MainForm()
    {
        _viewModel = Program.Services.GetRequiredService<GamePageViewModel>();
        _infoBarService = Program.Services.GetRequiredService<IInfoBarService>();
        
        InitializeComponent();
        SetupDataBindings();
        UpdateUIState();
    }

    private void InitializeComponent()
    {
        this.Text = "Codebreaker Game";
        this.Size = new Size(800, 600);
        this.MinimumSize = new Size(600, 400);
        this.StartPosition = FormStartPosition.CenterScreen;

        var mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(10)
        };
        
        // Configure row styles
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100)); // Start game / Peg selection
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Moves list
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30)); // Status
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); // Info bar

        // 1. Start game panel
        CreateStartGamePanel();
        mainLayout.Controls.Add(_startGamePanel, 0, 0);

        // 2. Peg selection panel (initially hidden)
        CreatePegSelectionPanel();
        mainLayout.Controls.Add(_pegSelectionPanel, 0, 0);

        // 3. Moves list
        _movesListBox = new ListBox
        {
            Dock = DockStyle.Fill,
            DrawMode = DrawMode.OwnerDrawVariable,
            ItemHeight = 40
        };
        _movesListBox.DrawItem += MovesListBox_DrawItem;
        _movesListBox.MeasureItem += MovesListBox_MeasureItem;
        mainLayout.Controls.Add(_movesListBox, 0, 1);

        // 4. Status panel
        var statusPanel = new Panel { Dock = DockStyle.Fill };
        _statusLabel = new Label
        {
            Dock = DockStyle.Left,
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleLeft
        };
        _progressBar = new ProgressBar
        {
            Dock = DockStyle.Fill,
            Style = ProgressBarStyle.Marquee,
            Visible = false
        };
        statusPanel.Controls.Add(_progressBar);
        statusPanel.Controls.Add(_statusLabel);
        mainLayout.Controls.Add(statusPanel, 0, 2);

        // 5. Info bar
        _infoBarTextBox = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            BackColor = Color.LightYellow
        };
        mainLayout.Controls.Add(_infoBarTextBox, 0, 3);

        this.Controls.Add(mainLayout);
    }

    private void CreateStartGamePanel()
    {
        _startGamePanel = new Panel { Dock = DockStyle.Fill };
        
        var label = new Label
        {
            Text = "Enter your name:",
            Location = new Point(10, 10),
            AutoSize = true
        };

        _usernameTextBox = new TextBox
        {
            Location = new Point(10, 35),
            Width = 300,
            Text = _viewModel.Name
        };
        _usernameTextBox.TextChanged += (s, e) => _viewModel.Name = _usernameTextBox.Text;

        _startGameButton = new Button
        {
            Text = "Start Game",
            Location = new Point(320, 33),
            Width = 120,
            Height = 25
        };
        _startGameButton.Click += async (s, e) => await StartGameAsync();

        _startGamePanel.Controls.Add(label);
        _startGamePanel.Controls.Add(_usernameTextBox);
        _startGamePanel.Controls.Add(_startGameButton);
    }

    private void CreatePegSelectionPanel()
    {
        _pegSelectionPanel = new Panel { Dock = DockStyle.Fill, Visible = false };
        
        var flowLayout = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            AutoScroll = true
        };

        _colorComboBoxes = new ComboBox[4]; // Default to 4 pegs
        
        for (int i = 0; i < 4; i++)
        {
            var comboBox = new ComboBox
            {
                Width = 100,
                Height = 30,
                Margin = new Padding(5),
                DropDownStyle = ComboBoxStyle.DropDownList,
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = 30
            };
            comboBox.DrawItem += ColorComboBox_DrawItem;
            _colorComboBoxes[i] = comboBox;
            flowLayout.Controls.Add(comboBox);
        }

        _setMoveButton = new Button
        {
            Text = "Set Move",
            Width = 100,
            Height = 30,
            Margin = new Padding(10, 5, 5, 5)
        };
        _setMoveButton.Click += async (s, e) => await SetMoveAsync();
        flowLayout.Controls.Add(_setMoveButton);

        _pegSelectionPanel.Controls.Add(flowLayout);
    }

    private void SetupDataBindings()
    {
        _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        _infoBarService.Messages.CollectionChanged += Messages_CollectionChanged;
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        this.Invoke(() =>
        {
            if (e.PropertyName == nameof(GamePageViewModel.GameStatus))
            {
                UpdateUIState();
            }
            else if (e.PropertyName == nameof(GamePageViewModel.GameMoves))
            {
                UpdateMovesList();
            }
            else if (e.PropertyName == nameof(GamePageViewModel.InProgress))
            {
                _progressBar.Visible = _viewModel.InProgress;
            }
            else if (e.PropertyName == nameof(GamePageViewModel.Game))
            {
                if (_viewModel.Game is not null)
                {
                    UpdatePegSelection();
                }
            }
        });
    }

    private void Messages_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        this.Invoke(() =>
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    _infoBarTextBox.AppendText(item?.ToString() + Environment.NewLine);
                }
            }
        });
    }

    private void UpdateUIState()
    {
        var gameStatus = _viewModel.GameStatus;
        _statusLabel.Text = gameStatus.ToString();

        switch (gameStatus)
        {
            case GameMode.NotRunning:
                _startGamePanel.Visible = true;
                _pegSelectionPanel.Visible = false;
                _movesListBox.Visible = false;
                break;
            case GameMode.Started:
            case GameMode.MoveSet:
                _startGamePanel.Visible = false;
                _pegSelectionPanel.Visible = true;
                _movesListBox.Visible = true;
                break;
            case GameMode.Won:
            case GameMode.Lost:
                _startGamePanel.Visible = false;
                _pegSelectionPanel.Visible = false;
                _movesListBox.Visible = true;
                ShowGameResult(gameStatus);
                break;
        }
    }

    private void UpdatePegSelection()
    {
        if (_viewModel.Game?.FieldValues is null) return;

        var colors = _viewModel.Game.FieldValues.ToArray();
        foreach (var comboBox in _colorComboBoxes)
        {
            comboBox.Items.Clear();
            foreach (var color in colors)
            {
                comboBox.Items.Add(color);
            }
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }
    }

    private void UpdateMovesList()
    {
        _movesListBox.Items.Clear();
        foreach (var move in _viewModel.GameMoves)
        {
            _movesListBox.Items.Add(move);
        }
    }

    private async Task StartGameAsync()
    {
        if (_viewModel.StartGameCommand.CanExecute(null))
        {
            await _viewModel.StartGameCommand.ExecuteAsync(null);
        }
    }

    private async Task SetMoveAsync()
    {
        if (_viewModel.SetMoveCommand.CanExecute(null))
        {
            // Update the selected fields from combo boxes
            for (int i = 0; i < _colorComboBoxes.Length && i < _viewModel.Fields.Count; i++)
            {
                if (_colorComboBoxes[i].SelectedItem is string selectedColor)
                {
                    _viewModel.Fields[i].Value = selectedColor;
                }
            }

            await _viewModel.SetMoveCommand.ExecuteAsync(null);
        }
    }

    private void ShowGameResult(GameMode gameMode)
    {
        string message = gameMode == GameMode.Won 
            ? "Congratulations! You won the game!" 
            : "Game over! You lost.";
        string title = gameMode == GameMode.Won ? "Victory!" : "Defeat";
        
        MessageBox.Show(message, title, MessageBoxButtons.OK, 
            gameMode == GameMode.Won ? MessageBoxIcon.Information : MessageBoxIcon.Exclamation);
    }

    private void ColorComboBox_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (sender is not ComboBox comboBox || e.Index < 0) return;

        e.DrawBackground();
        
        var colorName = comboBox.Items[e.Index]?.ToString() ?? "";
        var color = Helpers.ColorHelper.GetColorFromName(colorName);
        
        using (var brush = new SolidBrush(color))
        {
            var rect = new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Height - 4, e.Bounds.Height - 4);
            e.Graphics.FillEllipse(brush, rect);
        }
        
        using (var brush = new SolidBrush(e.ForeColor))
        {
            var textRect = new Rectangle(e.Bounds.X + e.Bounds.Height + 5, e.Bounds.Y, 
                e.Bounds.Width - e.Bounds.Height - 5, e.Bounds.Height);
            e.Graphics.DrawString(colorName, e.Font!, brush, textRect, 
                new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
        }
        
        e.DrawFocusRectangle();
    }

    private void MovesListBox_MeasureItem(object? sender, MeasureItemEventArgs e)
    {
        e.ItemHeight = 40;
    }

    private void MovesListBox_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (sender is not ListBox listBox || e.Index < 0 || e.Index >= listBox.Items.Count) return;

        e.DrawBackground();
        
        if (listBox.Items[e.Index] is Move move)
        {
            int x = e.Bounds.X + 5;
            int y = e.Bounds.Y + 5;
            int pegSize = 25;
            int spacing = 5;

            // Draw guess pegs
            for (int i = 0; i < move.GuessPegs.Length; i++)
            {
                var colorName = move.GuessPegs[i];
                var color = Helpers.ColorHelper.GetColorFromName(colorName);
                using (var brush = new SolidBrush(color))
                {
                    e.Graphics.FillEllipse(brush, x, y, pegSize, pegSize);
                }
                x += pegSize + spacing;
            }

            x += 10;

            // Draw key pegs
            var keyPegString = $"Correct: {move.KeyPegs.Count(k => k == "Black")}, Wrong Position: {move.KeyPegs.Count(k => k == "White")}";
            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(keyPegString, e.Font!, brush, x, y + 5);
            }
        }
        
        e.DrawFocusRectangle();
    }
}
