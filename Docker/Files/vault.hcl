api_addr      = "https://0.0.0.0:8200"
disable_mlock = false
ui = true

storage "raft" {
  path    = "/vault/file/"
  node_id = "vault_1"
}

listener "tcp" {
  address            = "0.0.0.0:8200"
  #change these if your certificate files are named differently
  tls_cert_file      = "/opt/vault/tls/vault-cert.pem"
  tls_key_file       = "/opt/vault/tls/vault-key.pem"
}