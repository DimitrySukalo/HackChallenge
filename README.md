# HackChallenge
This is a bot where you learn to decode messages and to get into someone else's computer. 

# Usage
To use this bot on your own, you need to specify your token and the path to the database.

# Commands
The bot has the following commands:
1. ls - shows the folders of the current directory
2. ls -la shows the folders of the current directory with desription
3. ping site.com - sends packets to a specific site
4. cd - changes to the selected directory
5. cd.. - goes back to the directory
6. /help - shows list of commands
7. smtp-check ip - shows connected users at the selected address
8. pwd - displays the path to the current directory
9. nmap ip - checks the address for open ports
10. netdiscover - shows devices that are with you on the same network
11. ifconfig - you can see your ip address
12. aireplay-ng - sends packets to the selected network
13. airmon-ng start wlan0 - changes wifi module mode to the MonitMode
14. airodump-ng wlan0mon - shows list of wifis
15. aircrack-ng - picks up a password in a wifi network
16. hydra test -P /root/wordlist ssh://192.168.60.50 where -P path to dictionary with passwords and ssh://192.168.60.50 - specifying the server 
and the victim's IP address and test - login
17. ssh ip@login password - where ip - ip-address of victim and password - password of the port
