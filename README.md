# vPilot Notify Plugin

A lightweight vPilot plugin that forwards your VATSIM **Private Messages**, **Radio Messages**, and **SELCAL Alerts** directly to your mobile phone using [ntfy.sh](https://ntfy.sh/).

---

## 🚀 How It Works

1. **vPilot** receives messages from VATSIM on your PC.
2. The **Plugin** sends a silent HTTP request to your custom `ntfy` topic.
3. The **ntfy App** on your mobile device pushes the alert instantly.

---

## 🛠️ Installation

1. Download `VpilotNotify.dll` from the [Releases](../../releases) section.
2. Copy `VpilotNotify.dll` into your vPilot plugins directory.
3. Launch **vPilot** once. The plugin will automatically generate a configuration file named `vpilotnotify-config.txt` in the same directory.
4. Close vPilot, open `vpilotnotify-config.txt` with any text editor, and replace the contents with your unique topic name (e.g., `my-vatsim-alerts-9823`).
5. Save the file.

---

## 📱 Mobile Setup (ntfy)

1. Download the **ntfy** app on your device ([Google Play Store](https://play.google.com/store/apps/details?id=io.heckel.ntfy) / [Apple App Store](https://apps.apple.com/app/ntfy/id1625396386)).
2. Open the app and tap **Subscribe to topic**.
3. Enter the **exact same topic name** specified in `vpilotnotify-config.txt`.

---

## 📄 License

This project is open-source and free to use.