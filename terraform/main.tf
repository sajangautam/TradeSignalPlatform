terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

provider "aws" {
  region = var.region
}

resource "aws_instance" "trade_signal_api" {
  ami           = "ami-0c7217cdde317cfec"
  instance_type = "t2.micro"
  key_name      = "trade-signal-key"     

  security_groups = [aws_security_group.api_sg.name]

  tags = {
    Name = "TradeSignalAPI"
  }
}

resource "aws_security_group" "api_sg" {
  name        = "trade-signal-api-sg"
  description = "Allow API and SSH access"

  ingress {
    from_port   = 5000  # API port
    to_port     = 5000
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 22  # SSH
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_s3_bucket" "trade_signal_data" {
  bucket = "trade-signal-data-${random_id.suffix.hex}"
  tags = {
    Purpose = "Store trade signal files"
  }
}

resource "random_id" "suffix" {
  byte_length = 4
}