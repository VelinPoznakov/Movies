import { Button, Flex, Image, message, Modal, Rate, Space, Typography } from "antd";
import type { MovieResponse } from "../types/moviesTypes";
import { useState } from "react";
import { useUpdateMovie } from "../Queries/Movies/movies";
import { useUser } from "../Contexts/useUser";
import { useNavigate } from "react-router";
import CommentsListComponent from "./CommentsListComponent";
import posterFallback from "../assets/image.png";

const { Title, Text } = Typography;

type MovieModalProps = {
  open: boolean;
  movie: MovieResponse | null;
  onClose: () => void;
}

function MovieDetailsComponent({ open, movie, onClose }: MovieModalProps) {
  const {user} = useUser();
  const navigate = useNavigate();
  const [stars, setStars] = useState(movie?.rating ?? 0);
  const {mutateAsync, isPending} = useUpdateMovie();
  const canRate = !!user;

  const handleRatingChange = async (value: number) => {
    setStars(value);
    if(!movie) return;

    try{
      await mutateAsync({
        id: movie.id,
        data: {
          title: movie.title,
          timeLong: movie.timeLong,
          rating: value,
          directorName: movie.director.name,
          genreName: movie.genre.name,
          releaseDate: movie.releaseDate
        }
      })
    }catch{
      message.error("Failed to set rating");
    }

  }

  return(
    <Modal
      open={open}
      onCancel={onClose}
      footer={null}
      destroyOnHidden
      centered
      styles={{
        body: {
          maxHeight: "80vh",
          overflow: "hidden",
        }
      }}
    >
      <div style={{ textAlign: "center" }}>
        <Image
          src={posterFallback}
          alt={movie?.title ?? "Movie poster"}
          width="100%"
          height={240}
          style={{ objectFit: "cover", borderRadius: 12, marginTop: 20 }}
          preview={false}
        />

        <Title level={2} style={{ marginTop: 22 }}>
          {movie?.title}
        </Title>
        <Text type="secondary">description</Text>

        <div style={{ marginTop: 12 }}>
          <Space orientation="vertical" style={{ width: "100%" }}>

            <div>
              <Flex justify="space-between" align="center">
                <Text strong>Duration:</Text>
                <Text>{movie?.timeLong}</Text>
              </Flex>
            </div>

            <div>
              <Flex justify="space-between" align="center">
                <Text strong>Director:</Text>
                <Text>{movie?.director.name}</Text>
              </Flex>
            </div>

            <div>
              <Flex justify="space-between" align="center">
                <Text strong>Genre:</Text>
                <Text>{movie?.genre.name}</Text>
              </Flex>
            </div>

            <div>
              <Flex justify="space-between" align="center">
                <Text strong>Release Date:</Text>
                <Text>{movie?.releaseDate ? new Date(movie?.releaseDate).toLocaleDateString("bg-BG", {
                  year: "numeric",
                  month: "2-digit",
                  day: "2-digit"
                }) : "-"}</Text>
              </Flex>
            </div>

            <div>
              <Flex justify="space-between" align="center">
                <Text strong>Rating: </Text>
                <Rate
                  value={stars}
                  onChange={(value) => {
                    if (!canRate) {
                      message.error("You must be logged in to rate a movie");
                      return navigate("/login");
                    }
                    handleRatingChange(value);
                  }}
                  disabled={isPending}
                />
              </Flex>
            </div>
          </Space>
        </div>

        <div style={{ maxHeight: 320, overflowY: "auto", paddingBottom: 16 }}>
          {movie && <CommentsListComponent movieId={movie.id} />}
        </div>

        {user?.username && user?.username === movie?.user?.username ? (
          <div>
            <Button type="primary">Edit</Button>
            <Button type="primary" danger>Delete</Button>
          </div>
        ) : null}

      </div>

    </Modal>
  )

}

export default MovieDetailsComponent;