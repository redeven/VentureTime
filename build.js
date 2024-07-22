const { existsSync, readFileSync, writeFileSync } = require('fs');
const path = require('path');
const { promisify } = require('util');
const exec = promisify(require('child_process').exec);

async function buildProject() {
    try {
        await exec('dotnet build -c Release');

        const pluginName = path.basename(__dirname);
        const buildPath = path.join('.', pluginName, 'bin', 'x64', 'Release', pluginName);
        const manifestPath = path.join(buildPath, `${pluginName}.json`);
        const downloadPath = `https://raw.githubusercontent.com/redeven/${pluginName}/master/${pluginName}/bin/x64/Release/${pluginName}/latest.zip`;
        if (existsSync(manifestPath)) {
            const manifestString = readFileSync(manifestPath, 'utf-8');
            const manifest = JSON.parse(manifestString);
            manifest.LastUpdated = Math.floor(new Date().getTime() / 1000);
            manifest.DownloadLinkInstall = downloadPath;
            manifest.DownloadLinkTesting = downloadPath;
            manifest.DownloadLinkUpdate = downloadPath;
            const repository = [manifest];
            writeFileSync('repository.json', JSON.stringify(repository, null, 4), 'utf-8');
        }
    } catch (e) {
        console.error(e);
    }
}

buildProject();
