import { Button, Flex, message, Modal, Rate, Space, Typography } from "antd";
import type { MovieResponse } from "../types/moviesTypes";
import { useState } from "react";
import { useUpdateMovie } from "../Queries/Movies/movies";
import { useUser } from "../Contexts/useUser";
import { useNavigate } from "react-router";

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
    >
      <div style={{ textAlign: "center" }}>
        <h1>IMAGE</h1>
        <Title level={2}>{movie?.title}</Title>
        <Text>description</Text>

        <div style={{ marginTop: 12 }}>
          <Space direction="vertical" style={{ width: "100%" }}>
            <Flex justify="space-between" align="center">
              <Text strong>Director:</Text>
              <Text>{movie?.director.name}</Text>
            </Flex>

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