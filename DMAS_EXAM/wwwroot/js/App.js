// ==========================================================================
// BATTLEGAME Admin Console - plain JavaScript client (no framework, no build step)
// Talks to the ASP.NET Core Web API controllers:
//   POST /api/registerplayer
//   POST /api/createasset
//   GET  /api/getassetsbyplayer
//   (+ utility endpoints: GET /api/players, GET /api/assets, POST /api/assignasset)
// ==========================================================================

function getBaseUrl() {
    const raw = document.getElementById("apiBaseUrl").value.trim();
    // strip trailing slash so "base + /path" never produces a double slash
    return raw.endsWith("/") ? raw.slice(0, -1) : raw;
}

async function apiRequest(path, options = {}) {
    const url = getBaseUrl() + path;
    const response = await fetch(url, {
        headers: { "Content-Type": "application/json" },
        ...options,
    });

    let body = null;
    const text = await response.text();
    if (text) {
        try {
            body = JSON.parse(text);
        } catch {
            body = text;
        }
    }

    if (!response.ok) {
        const message =
            (body && (body.message || body.title)) ||
            `Request failed with status ${response.status}`;
        throw new Error(message);
    }

    return body;
}

function showResult(elementId, message, isSuccess) {
    const el = document.getElementById(elementId);
    el.textContent = message;
    el.className = "small mt-2 " + (isSuccess ? "text-success" : "text-danger");
}

// ---------------------------------------------------------------------
// Connection check
// ---------------------------------------------------------------------
async function checkApiConnection() {
    const badge = document.getElementById("apiStatus");
    badge.textContent = "đang kiểm tra...";
    badge.className = "badge bg-secondary";
    try {
        await apiRequest("/players");
        badge.textContent = "kết nối OK";
        badge.className = "badge bg-success";
    } catch (err) {
        badge.textContent = "lỗi kết nối";
        badge.className = "badge bg-danger";
        console.error(err);
    }
}

// ---------------------------------------------------------------------
// Requirement 1: register player
// ---------------------------------------------------------------------
async function handleRegisterPlayer(event) {
    event.preventDefault();
    const form = event.target;
    const payload = {
        playerName: form.playerName.value,
        fullName: form.fullName.value,
        age: form.age.value,
        level: Number(form.level.value) || 0,
        email: form.email.value,
    };

    try {
        const player = await apiRequest("/registerplayer", {
            method: "POST",
            body: JSON.stringify(payload),
        });
        showResult(
            "registerPlayerResult",
            `✅ Đã tạo player #${player.playerId} - ${player.playerName}`,
            true
        );
        form.reset();
        form.level.value = 1;
        await loadPlayersIntoSelect();
    } catch (err) {
        showResult("registerPlayerResult", `❌ ${err.message}`, false);
    }
}

// ---------------------------------------------------------------------
// Requirement 2: create asset
// ---------------------------------------------------------------------
async function handleCreateAsset(event) {
    event.preventDefault();
    const form = event.target;
    const payload = {
        assetName: form.assetName.value,
        levelRequire: Number(form.levelRequire.value) || 0,
    };

    try {
        const asset = await apiRequest("/createasset", {
            method: "POST",
            body: JSON.stringify(payload),
        });
        showResult(
            "createAssetResult",
            `✅ Đã tạo asset #${asset.assetId} - ${asset.assetName}`,
            true
        );
        form.reset();
        form.levelRequire.value = 1;
        await loadAssetsIntoSelect();
    } catch (err) {
        showResult("createAssetResult", `❌ ${err.message}`, false);
    }
}

// ---------------------------------------------------------------------
// Utility: assign asset to player
// ---------------------------------------------------------------------
async function handleAssignAsset(event) {
    event.preventDefault();
    const form = event.target;
    const payload = {
        playerId: Number(form.playerId.value),
        assetId: Number(form.assetId.value),
    };

    try {
        await apiRequest("/assignasset", {
            method: "POST",
            body: JSON.stringify(payload),
        });
        showResult("assignAssetResult", "✅ Đã gán asset cho player", true);
        await loadReport();
    } catch (err) {
        showResult("assignAssetResult", `❌ ${err.message}`, false);
    }
}

// ---------------------------------------------------------------------
// Dropdown loaders
// ---------------------------------------------------------------------
async function loadPlayersIntoSelect() {
    const select = document.getElementById("selectPlayer");
    try {
        const players = await apiRequest("/players");
        select.innerHTML = players
            .map((p) => `<option value="${p.playerId}">#${p.playerId} - ${p.playerName}</option>`)
            .join("");
    } catch (err) {
        select.innerHTML = `<option value="">(không tải được danh sách player)</option>`;
    }
}

async function loadAssetsIntoSelect() {
    const select = document.getElementById("selectAsset");
    try {
        const assets = await apiRequest("/assets");
        select.innerHTML = assets
            .map((a) => `<option value="${a.assetId}">#${a.assetId} - ${a.assetName}</option>`)
            .join("");
    } catch (err) {
        select.innerHTML = `<option value="">(không tải được danh sách asset)</option>`;
    }
}

// ---------------------------------------------------------------------
// Requirement 3: report table
// ---------------------------------------------------------------------
async function loadReport() {
    const tbody = document.getElementById("reportTableBody");
    tbody.innerHTML = `<tr><td colspan="5" class="text-center text-muted">Đang tải...</td></tr>`;

    try {
        const rows = await apiRequest("/getassetsbyplayer");
        if (!rows.length) {
            tbody.innerHTML = `<tr><td colspan="5" class="text-center text-muted">Chưa có dữ liệu</td></tr>`;
            return;
        }
        tbody.innerHTML = rows
            .map(
                (r) => `
        <tr>
          <td>${r.no}</td>
          <td>${r.playerName}</td>
          <td>${r.level}</td>
          <td>${r.age}</td>
          <td>${r.assetName}</td>
        </tr>`
            )
            .join("");
    } catch (err) {
        tbody.innerHTML = `<tr><td colspan="5" class="text-center text-danger">❌ ${err.message}</td></tr>`;
    }
}

// ---------------------------------------------------------------------
// Wire everything up
// ---------------------------------------------------------------------
document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("formRegisterPlayer").addEventListener("submit", handleRegisterPlayer);
    document.getElementById("formCreateAsset").addEventListener("submit", handleCreateAsset);
    document.getElementById("formAssignAsset").addEventListener("submit", handleAssignAsset);
    document.getElementById("btnRefreshReport").addEventListener("click", loadReport);
    document.getElementById("btnCheckApi").addEventListener("click", checkApiConnection);

    loadPlayersIntoSelect();
    loadAssetsIntoSelect();
    loadReport();
    checkApiConnection();
});