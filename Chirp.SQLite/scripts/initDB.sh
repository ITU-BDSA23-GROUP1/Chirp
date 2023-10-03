#!/usr/bin/env bash
sqlite3 /tmp/chirp.db < ../Chirp.SQLite/data/schema.sql
sqlite3 /tmp/chirp.db < ../Chirp.SQLite/data/dump.sql
