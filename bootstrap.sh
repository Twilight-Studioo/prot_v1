#!/usr/bin/env bash

${UNITY_EXECUTABLE:-xvfb-run --auto-servernum --server-args='-screen 0 640x480x24' unity-editor} \
-batchmode -quit -projectPath $UNITY_DIR -executeMethod Packages.Rider.Editor.RiderScriptEditor.SyncSolution