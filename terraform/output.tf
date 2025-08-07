output "instance_public_ip" {
  value = aws_instance.trade_signal_api.public_ip
}