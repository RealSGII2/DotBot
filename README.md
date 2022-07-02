![Frame 14 (1)](https://user-images.githubusercontent.com/41650610/176984511-1bd2b53b-b7c4-4aee-bdc8-c9822c3a8abd.png)

<p float="left">
  <a href="https://discord.gg/2jKGg3n2bb" target="_blank">
    <img src="https://cdn.discordapp.com/attachments/992111983920939110/992632057677238302/Frame_15_1.png" height="44" />
  </a>
</p>

<h3 align="center">
  A free, open source Discord levelling bot.
</h3>

## Motivation

There's plenty of other levelling bots on Discord. Why make our own?

Many bots:
- Have their essential features behind paywalls
- Introduce their own bias on how levelling "should work"
- Have fancy "voter only" features

We decided to make our own bot to fix these:
- Everything you need is free
- Everything is customisable
  - In fact, if you can't find what you're looking for,
  you're completely allowed to clone and modify this bot
  (whilst following our [license](https://github.com/RealSGII2/DotBot/blob/master/LICENSE.txt))
- No paywalls, votewalls, etc.

## Getting Started

The bot is currently private. You may not invite it at this time.

### Documentation

No documentation yet.

### Questions & General Support

You may ask for support in our [Discord server](https://discord.gg/2jKGg3n2bb).

### Reporting Bugs & Submitting Feature Requests

You may report bugs or suggest new features by using the [issue tracker](https://github.com/RealSGII2/DotBot/issues).

Questions will be closed as they are to be posted in our support server, not the issue tacker.

## Contributing

Thanks wanting to contribute! Here are some of the things you'll need to get started:

### Project Structure

The different parts of the project are divided into different C# projects:
- `DotBot` is the program you run to start the bot
- `DotBot.Bot` is the code that runs the bot
- `DotBot.Shared` is the code that is shared between all of the projects
  - This is here because we plan on adding an API server too.
  
### Getting Started

To allow essential services to start, you need to configure tokens (like a Discord bot token).

You may do this by going into `DotBot.Shared`, and renaming `props.template.yml` to `props.yml`.
Then, change the the values in there. 

#### Starting

Run/debug `DotBot` to start the program.
